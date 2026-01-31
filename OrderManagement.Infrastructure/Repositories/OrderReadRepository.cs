public class OrderReadRepository {
  private readonly IDbConnectinon _connection;

  public OrderReadRepository(IDbConnection connection)
    {
        _connection = connection;
    }

  public async Task<IEnumerable<OrderListDto>> GetOrders(){
    return await _connection.QueryAsync<OrderListDto>(
      "SELECT Id, CreatedAt FROM Orders"
    );
  }
}