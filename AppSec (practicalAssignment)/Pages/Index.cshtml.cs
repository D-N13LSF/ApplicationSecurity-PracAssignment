using AppSec__practicalAssignment_.Controllers;
using AppSec__practicalAssignment_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace AppSec__practicalAssignment_.Pages
{
    [Authorize]
    [SessionTimeout]
    public class IndexModel : PageModel
    {
        private readonly UserManager<UserClass> _userManager;
        private readonly IDataProtector _protector;

        public IndexModel(UserManager<UserClass> userManager, IDataProtectionProvider dataProtectionProvider)
        {  
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector("MySecretKey"); // Use same key as in RegisterModel
        }

        public UserClass CurrentUser { get; private set; }
        public string DecryptedCreditCard { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User); // Get logged-in user
            if (user == null)
            {
                return RedirectToPage("/PageLogin/Login"); // Redirect to login if not found
            }

            // Decrypt credit card information
            try
            {
                DecryptedCreditCard = _protector.Unprotect(user.CreditCardNo);
            }
            catch (CryptographicException)
            {
                DecryptedCreditCard = "Error decrypting data.";
            }

            CurrentUser = user;
            return Page();
        }

        public IActionResult OnGetUserImage(string userId)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user?.userImagePath != null)
            {
                return File(user.userImagePath, "image/jpeg");
            }
            return NotFound();
        }   
    }
}
