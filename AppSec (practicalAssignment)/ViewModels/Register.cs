using System.ComponentModel.DataAnnotations;

namespace AppSec__practicalAssignment_.ViewModels
{
    public class Register
    {
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        public string CreditCardNo { get; set; }

        [Required, MinLength(8), MaxLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }


        [Required]
        [DataType(DataType.Text)]
        public string BillingAdd { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string ShipAdd { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required, MinLength(12)]
        [PasswordComplexity]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation does not match")]
        public string ConfirmPassword { get; set; }

		[Required]
		[AllowedExtensions(new[] { ".jpg", ".jpeg" }, ErrorMessage = "Only JPG files are allowed.")]
		public IFormFile? userImage { get; set; }
    }
}
