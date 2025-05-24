using Dev.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        public string ItemName { get; set; } = null!;

        public string? Description { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        public bool Availability { get; set; } = true;

        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; } = null!;

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
