namespace OrderManagement.Infrastructure.Repositories;

public class OrderRepository: IOrderRepository {

  private readonly OrdersDbContext _context;

  public OrderRepository(OrdersDbContext ctx){
    _context = ctx;
  }

  public async Task AddAsync(Order order){
    _context.Orders.Add(Order);
    await _context.SaveChangesAsync();
  }

  public Task<Order?> GetByIdAsync(Guid id){
    _context.Orders
    .Include(o => o.Items)
    .FirstOrDefaultAsync(o => o.od == id)''
  }
}