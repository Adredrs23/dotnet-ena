namespace OrderManagement.Infrastructure.Services.Auth;

using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using OrderManagement.Application.Interfaces.Auth;
using OrderManagement.Application.Queries.Auth;
using OrderManagement.Domain.Exceptions.Auth;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly string _googleClientId;

    public GoogleAuthService(IConfiguration configuration)
    {
        _googleClientId = configuration["Google:ClientId"]
            ?? throw new InvalidOperationException("Google ClientId not configured");
    }

    public async Task<GoogleTokenInfo> ValidateGoogleTokenAsync(string idToken)
    {
        try
        {
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, validationSettings);

            return new GoogleTokenInfo(
                Email: payload.Email,
                GoogleUserId: payload.Subject,
                Name: payload.Name,
                Picture: payload.Picture
            );
        }
        catch (InvalidJwtException ex)
        {
            throw new AuthException($"Invalid Google token: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new AuthException($"Google token validation failed: {ex.Message}");
        }
    }
}