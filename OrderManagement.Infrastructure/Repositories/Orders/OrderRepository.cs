namespace OrderManagement.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces.Orders;
using OrderManagement.Domain.Entities.Orders;
using OrderManagement.Infrastructure.Persistence.Orders;

public class OrderRepository : IOrderRepository
{

  private readonly OrdersDbContext _context;

  public OrderRepository(OrdersDbContext ctx)
  {
    _context = ctx;
  }

  public Task AddAsync(Order order)
  {
    _context.Orders.Add(order);
    // await _context.SaveChangesAsync();

    return Task.CompletedTask;
  }

  public Task<Order?> GetByIdAsync(Guid id)
  {
    return _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
  }
}