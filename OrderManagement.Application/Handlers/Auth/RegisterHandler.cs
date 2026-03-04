namespace OrderManagement.Application.Handlers.Auth;

using OrderManagement.Application.Commands.Auth;
using OrderManagement.Application.Interfaces.Auth;
using OrderManagement.Application.Queries.Auth;

public class RegisterHandler
{
    private readonly IAuthService _authService;

    public RegisterHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand command)
    {
        return await _authService.RegisterAsync(command);
    }
}