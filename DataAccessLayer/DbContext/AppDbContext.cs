using BusinessLogicLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderStatus)
                .HasConversion<string>();
            entity.Property(e => e.PaymentStatus)
                .HasConversion<string>();
            entity.Property(e => e.FulfillmentStatus)
                .HasConversion<string>();
        });
    }
}