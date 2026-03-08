namespace OrderManagement.Application.Handlers.Auth;

using OrderManagement.Application.Commands.Orders;
using OrderManagement.Application.Interfaces.Orders;
using OrderManagement.Application.Interfaces.Common;
using OrderManagement.Domain.Entities.Orders;

public class CreateOrderHandler
{
  private readonly IOrderRepository _repository;
  private readonly IUnitOfWork _uow;

  public CreateOrderHandler(IOrderRepository repository, IUnitOfWork uow)
  {
    _repository = repository;
    _uow = uow;
  }

  public async Task Handle(CreateOrderCommand command)
  {
    var order = new Order(command.Id);

    foreach (var item in command.Items)
    {
      order.AddItem(item.ProductId, item.Quantity, item.Price);
    }

    await _repository.AddAsync(order);
    await _uow.CommitAsync();
  }
}