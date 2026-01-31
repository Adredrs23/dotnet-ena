namespace OrderManagement.Application.Interfaces;

using OrderManagement.Domain.Entities;

public interface IOrderRepository
{
  Task AddAsync(Order order);
  Task<Order?> GetByIdAsync(Guid Id);
}