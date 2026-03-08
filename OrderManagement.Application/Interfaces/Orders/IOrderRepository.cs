namespace OrderManagement.Application.Interfaces.Orders;

using OrderManagement.Domain.Entities.Orders;

public interface IOrderRepository
{
  Task AddAsync(Order order);
  Task<Order?> GetByIdAsync(Guid Id);
}