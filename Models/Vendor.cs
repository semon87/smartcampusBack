using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev.Models
{
    public class Vendor
    {
        [Key]
        public int VendorId { get; set; }

        public string? VendorName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;  

        public bool IsOpen { get; set; } = true;

        public string? Schedule { get; set; }

        public string? distribute_item { get; set; }

        // Foreign key to User
        [Required]
        public int UserId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public ICollection<MenuItem>? MenuItems { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
