namespace OrderManagement.Application.Interfaces.Auth;

using OrderManagement.Application.Commands.Auth;
using OrderManagement.Application.Queries.Auth;

public interface IAuthService
{

    public Task<AuthResponseDto> RegisterAsync(RegisterCommand command);

    public Task<AuthResponseDto> LoginAsync(LoginCommand command);

    public Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenCommand command);

    public Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginCommand command);

    public Task LogoutAsync(LogoutCommand command);

}