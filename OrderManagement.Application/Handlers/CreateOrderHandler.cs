namespace OrderManagement.Application.Handlers;

using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Application.Commands;

public class CreateOrderHandler {
  private readonly IOrderRepository _repository;

  public CreateOrderHandler (IOrderRepository repository){
    _repository = repository;
  }

  public async Task Handle (CreateOrderCommand command){
    var order = new Order(command.Id);
    await _repository.AddAsync(order);
  }
}