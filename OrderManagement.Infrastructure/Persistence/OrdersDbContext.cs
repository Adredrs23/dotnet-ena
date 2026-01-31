namespace OrderManagement.Infrastructure.Persistence;


public class OrdersDbContext: DbContext {
  public DbSet<Order> Orders  => Set<Order>();

  public OrdersDbContext(DbContextOptions<OrdersDbContext> options): base {options} { }
}