namespace OrderManagement.Application.Handler;

public class CreateOrderHandler {
  private readonly IOrderRepository _repository;

  public CreateOrderHandler (IOrderRepository repository){
    _repository = repository;
  }

  public async Task Handle (CreateOrderCommand command){
    var order = new Order(command.Id);
    await _repository.addAsync(order);
  }
}