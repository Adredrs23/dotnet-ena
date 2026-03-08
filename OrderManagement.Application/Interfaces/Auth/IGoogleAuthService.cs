using OrderManagement.Application.Queries.Auth;

namespace OrderManagement.Application.Interfaces.Auth;


public interface IGoogleAuthService
{
    Task<GoogleTokenInfo> ValidateGoogleTokenAsync(string idToken);
}



