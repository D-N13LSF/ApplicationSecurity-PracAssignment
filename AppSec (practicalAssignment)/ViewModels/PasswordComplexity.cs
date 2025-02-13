using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AppSec__practicalAssignment_.ViewModels
{
    public class PasswordComplexity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Password is required.");
            }
            if (password.Length < 8)
            {
                return new ValidationResult("Password must be at least 8 characters long.");
            }

            // Check for combination of lowercase, uppercase, numbers, and special characters
            if (!Regex.IsMatch(password, @"[a-z]") ||
                !Regex.IsMatch(password, @"[A-Z]") ||
                !Regex.IsMatch(password, @"[0-9]") ||
                !Regex.IsMatch(password, @"[\W_]"))
            {
                return new ValidationResult("Password must contain lowercase, uppercase, numbers, and special characters.");
            }

            return ValidationResult.Success;
        }
    }
}

