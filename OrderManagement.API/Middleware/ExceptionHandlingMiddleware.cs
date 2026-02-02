using OrderManagement.Domain.Exceptions;

public class ExceptionHandlingMiddleware
{

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                message = ex.Message,
                type = "DomainError"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            // await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));

            await context.Response.WriteAsJsonAsync(new
            {
                message = "An unexpected error occured",
                type = "ServerError"
            });
        }
    }
}