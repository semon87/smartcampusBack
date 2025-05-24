using Dev.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public enum OrderStatus
    {
        Pending,
        Preparing,
        Ready,
        Shipped
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        // Navigation properties
        [ForeignKey("StudentId")]
        public User Student { get; set; } = null!;

        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; } = null!;

        public ICollection<OrderItem>? OrderItems { get; set; }

        public PaymentTransaction? PaymentTransaction { get; set; }
    }
}
