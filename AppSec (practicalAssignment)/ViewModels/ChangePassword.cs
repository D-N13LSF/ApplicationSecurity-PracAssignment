using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace AppSec__practicalAssignment_.ViewModels
{
    public class ChangePassword
    {

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [PasswordComplexity]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } 

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]   
        [Compare(nameof(NewPassword), ErrorMessage = "Password and confirmation does not match")]
        public string ConfirmPassword { get; set; } 
    }
}
