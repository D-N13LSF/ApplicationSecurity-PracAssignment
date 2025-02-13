using AppSec__practicalAssignment_.Models;
using AppSec__practicalAssignment_.Services;
using AppSec__practicalAssignment_.ViewModels;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Claims;

namespace AppSec__practicalAssignment_.Pages.PageLogin
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<UserClass> signInManager;
		private readonly UserManager<UserClass> userManager;
		private readonly AuthenDbContext _context;
		private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public Login LModel { get; set; }

        public LoginModel(SignInManager<UserClass> signInManager, UserManager<UserClass> userManager, AuthenDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.signInManager = signInManager;
			this.userManager = userManager;
            this.configuration = configuration;
			_context = context;
            _httpContextAccessor = httpContextAccessor;
        } 

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(LModel.Email);

                //Ensures Captcha is valid 
                string googleRecaptchaToken = Request.Form["g-recaptcha-response"].ToString();
                string secretKey = configuration["GoogleReCaptcha:SecretKey"];
                string verificationUrl = configuration["GoogleReCaptcha:VerificationUrl"];
                bool isValid = await ReCaptchaService.verifyReCaptchaV3(googleRecaptchaToken, secretKey, verificationUrl);

                //No user
                if (user == null)
                {
                    ModelState.AddModelError("", "Username or Password incorrect");
                    return Page();
                }

                //Locked out
                if (await userManager.IsLockedOutAsync(user))
                {
                    ModelState.AddModelError("", "Account locked due to multiple failed login attempts.");
                    return Page();
                }

                var result = await signInManager.PasswordSignInAsync(user, LModel.Password, LModel.RememberMe, lockoutOnFailure: true);

                //Validating Captcha
                if (!isValid)
                {
					ModelState.AddModelError("", "Invalid Captcha");
					return Page();
				}

                if (result.Succeeded)
                {
                    // Store secure session values
                    _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id);
                    HttpContext.Session.SetString("AuthToken", Guid.NewGuid().ToString()); // Prevent session fixation
                    HttpContext.Session.SetString("LastLoginTime", DateTime.UtcNow.ToString());

                    // Store device/session tracking info in DB
                    await TrackLoginSession(user.Id, HttpContext.Connection.RemoteIpAddress?.ToString());

                    // Redirect to homepage
                    return RedirectToPage("/Index");
                }
                else
                {
                    // Handle failed attempts
                    var failedAttempts = await userManager.GetAccessFailedCountAsync(user);
                    if (failedAttempts >= 6)
                    {
                        await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(15));
                        ModelState.AddModelError(string.Empty, "Account locked due to multiple failed login attempts.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Username or Password incorrect");
                    }

                    return Page();
                }
            }
            return Page();
        }

        //private void LogActivity(string userId, string activity)
        //{
        //    var auditLog = new AuditLog
        //    {
        //        UserId = userId,
        //        Activity = activity,
        //        Timestamp = DateTime.UtcNow
        //    };
        //    _context.AuditLogs.Add(auditLog);
        //    _context.SaveChanges();
        //}

        private async Task TrackLoginSession(string userId, string ipAddress)
        {
            var existingSession = await _context.UserSessions
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (existingSession != null)
            {
                // Remove previous session if user logs in again
                _context.UserSessions.Remove(existingSession);
            }

            var session = new UserSession
            {
                UserId = userId,
                SessionId = HttpContext.Session.GetString("AuthToken"),
                IPAddress = ipAddress,
                LastActivity = DateTime.UtcNow
            };

            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();
        }

        public void OnGet()
        {
        }
    }
}
