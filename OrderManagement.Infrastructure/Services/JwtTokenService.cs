using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application.Interfaces.Auth;
using OrderManagement.Application.Queries.Auth;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpiryMinutes;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration.GetValue<string>("Jwt:SecretKey") ?? throw new ArgumentNullException("Jwt:SecretKey");
        _issuer = _configuration.GetValue<string>("Jwt:Issuer") ?? "OrderManagementApi";
        _audience = _configuration.GetValue<string>("Jwt:Audience") ?? "OrderManagementClient";
        _accessTokenExpiryMinutes = int.Parse(_configuration.GetValue<string>("Jwt:AccessTokenExpiryInMinutes") ?? "15");
    }

    public string GenerateAccessToken(string userId, string email, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),     // Subject (user ID)
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID (unique identifier)
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()) // Issued At
        };

        // Add roles as claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Create signing credentials
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create token
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
            signingCredentials: credentials
        );

        // Serialize to string
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        // Generate 32 cryptographically random bytes
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        // Convert to Base64 string
        return Convert.ToBase64String(randomBytes);
    }

    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hashBytes);
    }

    public TokenValidationResultDto ValidateAccessToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ClockSkew = TimeSpan.Zero // No tolerance for clock differences
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            // Extract claims
            var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            var roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

            return new TokenValidationResultDto(
                IsValid: true,
                UserId: userId,
                Email: email,
                Roles: roles,
                ErrorMessage: null
            );
        }
        catch (SecurityTokenExpiredException)
        {
            return new TokenValidationResultDto(
                IsValid: false,
                UserId: null,
                Email: null,
                Roles: null,
                ErrorMessage: "Token has expired"
            );
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return new TokenValidationResultDto(
                IsValid: false,
                UserId: null,
                Email: null,
                Roles: null,
                ErrorMessage: "Invalid token signature"
            );
        }
        catch (Exception ex)
        {
            return new TokenValidationResultDto(
                IsValid: false,
                UserId: null,
                Email: null,
                Roles: null,
                ErrorMessage: $"Token validation failed: {ex.Message}"
            );
        }
    }

}