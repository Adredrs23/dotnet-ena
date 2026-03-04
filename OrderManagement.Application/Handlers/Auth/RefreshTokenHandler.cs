namespace OrderManagement.Application.Handlers.Auth;

using OrderManagement.Application.Commands.Auth;
using OrderManagement.Application.Interfaces.Auth;
using OrderManagement.Application.Queries.Auth;

public class RefreshTokenHandler
{
    private readonly IAuthService _authService;

    public RefreshTokenHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand command)
    {
        return await _authService.RefreshTokenAsync(command);
    }
}