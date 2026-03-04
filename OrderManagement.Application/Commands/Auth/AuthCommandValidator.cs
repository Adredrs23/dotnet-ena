namespace OrderManagement.Application.Commands.Auth;

using FluentValidation;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8).WithMessage("Password must be at least 8 characters long");
        RuleFor(c => c.ConfirmPassword).NotEmpty().Equal(c => c.Password).WithMessage("Passwords do not match");
    }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8).WithMessage("Password must be at least 8 characters long");
    }
}

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(c => c.RefreshToken).NotEmpty().WithMessage("Refresh token is required");
    }
}

public class GoogleLoginCommandValidator : AbstractValidator<GoogleLoginCommand>
{
    public GoogleLoginCommandValidator()
    {
        RuleFor(c => c.GoogleIdToken).NotEmpty().WithMessage("Google ID token is required");
    }
}

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(c => c.RefreshToken).NotEmpty().WithMessage("Refresh token is required");
    }
}