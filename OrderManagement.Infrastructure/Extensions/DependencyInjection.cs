
namespace OrderManagement.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Handlers.Auth;
using OrderManagement.Application.Interfaces.Common;
using OrderManagement.Application.Interfaces.Orders;
using OrderManagement.Infrastructure.Persistence.Common;
using OrderManagement.Infrastructure.Persistence.Orders;
using OrderManagement.Infrastructure.Repositories;


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
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<RefreshTokenHandler>();
        services.AddScoped<GoogleLoginHandler>();
        services.AddScoped<LogoutHandler>();
        return services;
    }

}