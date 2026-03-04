
namespace OrderManagement.Application.Commands.Auth;

public record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword
);

public record LoginCommand(
    string Email,
    string Password
);

public record RefreshTokenCommand(
    string RefreshToken
);

public record GoogleLoginCommand(
    string GoogleIdToken
);

public record LogoutCommand(
    string RefreshToken
);