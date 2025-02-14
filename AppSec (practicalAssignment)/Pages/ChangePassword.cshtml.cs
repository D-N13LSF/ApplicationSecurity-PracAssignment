using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AppSec__practicalAssignment_.Models;
using AppSec__practicalAssignment_.ViewModels;

namespace AppSec__practicalAssignment_.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<UserClass> _userManager;
        private readonly SignInManager<UserClass> _signInManager;

        [BindProperty]
        public ChangePassword PassChangeModel { get; set; }

        public ChangePasswordModel(UserManager<UserClass> userManager, SignInManager<UserClass> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Verify the old password
            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, PassChangeModel.OldPassword);
            if (!isOldPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "The current password is incorrect.");
                return Page();
            }

            // Change the password
            var result = await _userManager.ChangePasswordAsync(user, PassChangeModel.OldPassword, PassChangeModel.NewPassword);
            if (result.Succeeded)
            {
                // Re-sign in the user to refresh the authentication cookie
                await _signInManager.RefreshSignInAsync(user);

                // Redirect to a confirmation page or homepage
                return RedirectToPage("/Index");
            }

            // Handle errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}