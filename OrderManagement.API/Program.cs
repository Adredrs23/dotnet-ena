using FluentValidation;
using FluentValidation.AspNetCore;
using OrderManagement.Application.Commands.Auth;
using OrderManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RefreshTokenCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GoogleLoginCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LogoutCommandValidator>();

builder.Services.AddInfrastructure(builder.Configuration); ;

builder.Services.AddRepositories();

builder.Services.AddHandlers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
