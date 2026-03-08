namespace OrderManagement.Application.Commands.Orders;

using OrderManagement.Application.Queries.Orders;

public record CreateOrderCommand(Guid Id, List<CreateOrderItemDto> Items);