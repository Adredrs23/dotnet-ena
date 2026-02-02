using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Handlers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Infrastructure.Persistence;
using OrderManagement.Infrastructure.Repositories;

namespace OrderManagement.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrdersDbContext>(options =>
            // options.UseSqlServer(configuration.GetConnectionString("OrdersDb")));
            options.UseSqlite(configuration.GetConnectionString("OrdersDb")));
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderHandler>();
        return services;
    }


    // public static IServiceCollection AddServices(this IServiceCollection services) {
    //     services.AddScoped<IOrderService, OrderService>();
    //     return services;
    // }
}