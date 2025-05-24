using System;
using System.ComponentModel.DataAnnotations;

namespace Dev.Models
{

    public class User
    {
        [Key]
        public int UserId { get; set; }  // Primary key

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Department { get; set; } = null!;

        public string? Address { get; set; }

        [Required]
        public string StudentId { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;  // Hash before storing
    }
}
