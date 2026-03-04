namespace OrderManagement.Application.Handlers.Auth;

using OrderManagement.Application.Commands.Auth;
using OrderManagement.Application.Interfaces.Auth;
using OrderManagement.Application.Queries.Auth;

public class GoogleLoginHandler
{
    private readonly IAuthService _authService;

    public GoogleLoginHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(GoogleLoginCommand command)
    {
        return await _authService.GoogleLoginAsync(command);
    }
}