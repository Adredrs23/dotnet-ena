using OrderManagement.Application.Interfaces;

namespace OrderManagement.Infrastructure.Persistence;

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