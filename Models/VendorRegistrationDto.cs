using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Dev.Dtos
{
    public class VendorRegistrationDto
    {
        [Required]
        public string VendorName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = null!;

        public bool IsOpen { get; set; } = true;

        public string? Schedule { get; set; }

        public string? DistributeItem { get; set; }
    }
}
