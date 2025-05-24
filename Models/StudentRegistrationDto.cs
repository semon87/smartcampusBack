using System.ComponentModel.DataAnnotations;

namespace Dev.Dtos
{
    public class StudentRegistrationDto
    {
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
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = null!;
    }
}
