namespace OrderManagement.Infrastructure.Persistence.Orders;

using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities.Orders;
using OrderManagement.Infrastructure.Configurations.Orders;

public class OrdersDbContext : DbContext
{
  public DbSet<Order> Orders => Set<Order>();

  public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderConfiguration).Assembly);
  }
}
