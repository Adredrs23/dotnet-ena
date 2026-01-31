namespace OrderManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Configurations;

public class OrdersDbContext : DbContext
{
  public DbSet<Order> Orders => Set<Order>();

  public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderConfiguration).Assembly);
  }
}
