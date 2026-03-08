namespace OrderManagement.Application.Queries.Orders;

public record CreateOrderItemDto(Guid ProductId, int Quantity, decimal Price);