namespace OrderManagement.Application.Handlers;

using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Application.Commands;

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