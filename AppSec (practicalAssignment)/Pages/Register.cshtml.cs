using AppSec__practicalAssignment_.Models;
using AppSec__practicalAssignment_.ViewModels;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net;

using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;

namespace AppSec__practicalAssignment_.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<UserClass> userManager { get; }
        private SignInManager<UserClass> signInManager { get; }
		private RoleManager<IdentityRole> roleManager { get; }

		private IWebHostEnvironment _webHostEnvironment;
        private readonly IDataProtector _protector;


        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(
			UserManager<UserClass> userManager, SignInManager<UserClass> signInManager, 
			RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment, 
			IDataProtectionProvider dataProtectionProvider)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
			this.roleManager = roleManager;
			_webHostEnvironment = webHostEnvironment;
            _protector = dataProtectionProvider.CreateProtector("MySecretKey");
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{

				byte[]? imageBytes = null;
				if (RModel.userImage != null)
				{
					using (var memoryStream = new MemoryStream())
					{
						await RModel.userImage.CopyToAsync(memoryStream);
						imageBytes = memoryStream.ToArray();
					}
				}

                var user = new UserClass
				{
					FirstName = RModel.FirstName,
					LastName = RModel.LastName,
                    CreditCardNo = _protector.Protect(RModel.CreditCardNo),
                    PhoneNumber = RModel.MobileNo,
					BillingAdd = RModel.BillingAdd,
					ShipAdd = RModel.ShipAdd,
					UserName = RModel.Email,
					Email = RModel.Email,
					userImagePath = imageBytes
				};

				IdentityRole role = await roleManager.FindByIdAsync("Admin");
				if (role == null)
				{
					IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Admin"));
					if (!result2.Succeeded)
					{
						ModelState.AddModelError("", "Create role admin failed");
					}
				}

				var result = await userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{
					//
					result = await userManager.AddToRoleAsync(user, "Admin");

					await signInManager.SignInAsync(user, false);
					return RedirectToPage("/Login");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("",
					error.Description);
				}
			}
			return Page();
		}


		public void OnGet()
        {
        }
    }
}
