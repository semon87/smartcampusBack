using Dev.Models;
using Microsoft.EntityFrameworkCore;
using Dev.Models;

namespace Dev.Data
{
    public class SmartCampusContext : DbContext
    {
        public SmartCampusContext(DbContextOptions<SmartCampusContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Vendor> Vendors { get; set; } = null!;
        public DbSet<MenuItem> MenuItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure enum properties to store as strings (optional, but improves readability in DB)

            modelBuilder
                .Entity<Order>()
                .Property(o => o.PaymentStatus)
                .HasConversion<string>();

            modelBuilder
                .Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion<string>();

            modelBuilder
                .Entity<PaymentTransaction>()
                .Property(p => p.Status)
                .HasConversion<string>();

            // Additional configurations can go here as needed

            // Example: Composite Unique Index on Vendor.UserId to enforce one vendor per user
            modelBuilder.Entity<Vendor>()
                .HasIndex(v => v.UserId)
                .IsUnique();

            // Configure cascade delete rules if necessary (optional, default EF behavior is usually fine)
        }
    }
}
