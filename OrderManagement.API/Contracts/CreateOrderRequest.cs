namespace OrderManagement.API.Contracts;

public record CreateOrderRequest(List<CreateOrderItemRequest> Items);

public record CreateOrderItemRequest(Guid ProductId, int Quantity, decimal Price);