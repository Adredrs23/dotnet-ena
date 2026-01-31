using OrderManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddRepositories();

builder.Services.AddHandlers();

var app = builder.Build();

app.MapControllers();

app.Run();
