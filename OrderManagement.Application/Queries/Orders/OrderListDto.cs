namespace OrderManagement.Application.Queries.Orders;

public class OrderListDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
}