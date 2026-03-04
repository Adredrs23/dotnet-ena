namespace OrderManagement.Application.Interfaces.Auth;

using OrderManagement.Application.Queries.Auth;

public interface ITokenService
{
    //  Why do I need IList<string> roles?
    public string GenerateAccessToken(string userId, string email, IList<string> roles);

    public string GenerateRefreshToken();

    public string HashToken(string token);

    public TokenValidationResultDto ValidateAccessToken(string token);
}