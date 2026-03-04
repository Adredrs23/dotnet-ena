namespace OrderManagement.Application.Handlers.Auth;

using OrderManagement.Application.Commands.Auth;
using OrderManagement.Application.Interfaces.Auth;
using OrderManagement.Application.Queries.Auth;

public class LoginHandler
{
    private readonly IAuthService _authService;

    public LoginHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand command)
    {
        return await _authService.LoginAsync(command);
    }
}