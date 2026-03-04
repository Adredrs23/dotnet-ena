namespace OrderManagement.Application.Handlers.Auth;

using OrderManagement.Application.Commands.Auth;
using OrderManagement.Application.Interfaces.Auth;

public class LogoutHandler
{
    private readonly IAuthService _authService;

    public LogoutHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task Handle(LogoutCommand command)
    {
        await _authService.LogoutAsync(command);
    }
}