namespace OrderManagement.Infrastructure.Repositories.Orders;

using System.Data;
using Dapper;
using OrderManagement.Application.Queries.Orders;

public class OrderReadRepository
{
  private readonly IDbConnection _connection;

  public OrderReadRepository(IDbConnection connection)
  {
    _connection = connection;
  }

  public async Task<IEnumerable<OrderListDto>> GetOrders()
  {
    return await _connection.QueryAsync<OrderListDto>(
      "SELECT Id, CreatedAt FROM Orders"
    ) ?? Enumerable.Empty<OrderListDto>();
  }
}