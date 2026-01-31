namespace OrderManagement.Domain.Entities;
using OrderManagement.Domain.Exceptions;

public class Order
{
  public Guid Id {get; private set;}
  public DateTime CreatedAt {get; private set;}

  private readonly List<OrderItem> _items = new ();
  public  IReadOnlyCollection<OrderItem> Items => _items;

  private Order () {}

  public Order (Guid id){
    Id =  id;
    CreatedAt = DateTime.UtcNow;
  }

  public void AddItem(Guid productId, int quantity, decimal price ){
    if (quantity <0){
      throw new DomainException("Quantity cannot be less than zero");
    }

    _items.Add(new OrderItem(productId,quantity,price));
  }

  public decimal totalAmount ( ){
    return _items.Sum(i => i.Quantity * i.Price);
  } 
}
