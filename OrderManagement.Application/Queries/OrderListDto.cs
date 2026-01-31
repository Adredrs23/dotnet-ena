namespace OrderManagement.Application.Queries;

public class OrderListDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
}