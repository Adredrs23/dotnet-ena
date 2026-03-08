namespace OrderManagement.Infrastructure.Persistence.Common;

using OrderManagement.Application.Interfaces.Common;
using OrderManagement.Infrastructure.Persistence.Orders;

public class UnitOfWork : IUnitOfWork
{

    private readonly OrdersDbContext _context;

    public UnitOfWork(OrdersDbContext context)
    {
        _context = context;
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}