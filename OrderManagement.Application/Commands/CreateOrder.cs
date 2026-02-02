namespace OrderManagement.Application.Commands;

public record CreateOrderCommand(Guid Id, List<CreateOrderItemDto> Items);