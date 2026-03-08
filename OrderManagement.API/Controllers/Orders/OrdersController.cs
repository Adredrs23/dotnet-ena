namespace OrderManagement.API.Controllers.Orders;

using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Handlers;
using OrderManagement.Application.Commands;
using OrderManagement.API.Contracts.Orders;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _handler;

    public OrdersController(CreateOrderHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var id = Guid.NewGuid();

        var items = request.Items.Select(
            i => new CreateOrderItemDto(i.ProductId, i.Quantity, i.Price)
        ).ToList();

        await _handler.Handle(new CreateOrderCommand(id, items));
        return Ok(id);
    }
}


