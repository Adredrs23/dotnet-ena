namespace OrderManagement.API.Contracts.Orders;

public record CreateOrderRequest(List<CreateOrderItemRequest> Items);

public record CreateOrderItemRequest(Guid ProductId, int Quantity, decimal Price);