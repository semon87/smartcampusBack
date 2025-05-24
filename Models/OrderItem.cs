using Dev.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int MenuItemId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal PriceAtOrder { get; set; }

        // Navigation properties
        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;

        [ForeignKey("MenuItemId")]
        public MenuItem MenuItem { get; set; } = null!;
    }
}
