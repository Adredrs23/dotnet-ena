namespace OrderManagement.API.Controllers;

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
    public async Task<IActionResult> Create()
    {
        var id = Guid.NewGuid();
        await _handler.Handle(new CreateOrderCommand(id));
        return Ok(id);
    }
}
