using Dev.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev.Models
{
    public enum TransactionStatus
    {
        Pending,
        Success,
        Failed
    }

    public class PaymentTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int OrderId { get; set; }

        public string? PaymentGatewayId { get; set; }

        public TransactionStatus Status { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime TransactionTime { get; set; } = DateTime.UtcNow;

        public bool CallbackReceived { get; set; } = false;

        // Navigation properties
        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;
    }
}
