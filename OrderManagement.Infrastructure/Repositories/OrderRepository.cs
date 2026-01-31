namespace OrderManagement.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Persistence;

public class OrderRepository : IOrderRepository
{

  private readonly OrdersDbContext _context;

  public OrderRepository(OrdersDbContext ctx)
  {
    _context = ctx;
  }

  public async Task AddAsync(Order order)
  {
    _context.Orders.Add(order);
    await _context.SaveChangesAsync();
  }

  public async Task<Order?> GetByIdAsync(Guid id)
  {
    return await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
  }
}