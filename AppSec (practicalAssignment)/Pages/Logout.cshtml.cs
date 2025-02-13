using AppSec__practicalAssignment_.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AppSec__practicalAssignment_.Pages.PageLogin
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<UserClass> signInManager;
        private readonly AuthenDbContext _context;

        public LogoutModel(SignInManager<UserClass> signInManager, AuthenDbContext context)
        {
            this.signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var sessionId = HttpContext.Session.GetString("AuthToken");

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(sessionId))
            {
                // Remove session from database
                var userSession = await _context.UserSessions
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.SessionId == sessionId);

                if (userSession != null)
                {
                    _context.UserSessions.Remove(userSession);
                    await _context.SaveChangesAsync();
                }
            }

            // Clear session and sign out
            HttpContext.Session.Clear();
            await signInManager.SignOutAsync();

            return RedirectToPage("Login");
        }

        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("Index");
        }

        public void OnGet()
        {
        }
    }
}
