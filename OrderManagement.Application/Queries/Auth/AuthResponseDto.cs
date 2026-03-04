namespace OrderManagement.Application.Queries.Auth;

public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserDto User
);

public record UserDto(
    string Id,
    string Email,
    string[] Roles
);

public record TokenValidationResultDto(
    bool IsValid,
    string? UserId,
    string? Email,
    string[]? Roles,
    string? ErrorMessage
);

public record GoogleTokenInfo(
    string Email,
    string GoogleUserId,
    string Name,
    string Picture
);