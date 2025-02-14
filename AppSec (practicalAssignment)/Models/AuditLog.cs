using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppSec__practicalAssignment_.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }  // Primary key

        [Required]
        public string UserId { get; set; }  // ID of the user performing the action

        [Required]
        public string Activity { get; set; }  // Action performed (e.g., "Login Success", "Failed Login")

        public string IPAddress { get; set; }  // IP Address of the user

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;  // When the action occurred
    }
}
