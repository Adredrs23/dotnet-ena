namespace OrderManagement.Application.Interfaces.Auth;

using OrderManagement.Application.Queries.Auth;


public interface IGoogleTokenService
{
    public Task<GoogleTokenInfo> ValidateGoogleTokenAsync(string idToken);
}