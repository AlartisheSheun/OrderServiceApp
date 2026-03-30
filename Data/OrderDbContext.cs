using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;

namespace OrderAPI.Data;

// Order management using PostgreSQL
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Amount)
                .HasPrecision(18, 2);

            entity.Property(e => e.Status)
                .HasConversion<string>()
                .HasDefaultValue(OrderStatus.Pending);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.ToTable("Orders");
        });
    }
}
