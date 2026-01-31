namespace OrderManagement.Application.Interfaces;

public interface IOrderRepository{
  Task AddAsync(Order order);
  Task GetByIdAsync (Guid Id);
}