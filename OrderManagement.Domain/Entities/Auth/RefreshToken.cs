using OrderManagement.Domain.Exceptions.Auth;

namespace OrderManagement.Domain.Entities.Auth;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    public string TokenHash { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public Boolean IsRevoked { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    private RefreshToken() { }

    public RefreshToken(string userId, string tokenHash, int expiryDays = 7)
    {
        if (string.IsNullOrEmpty(userId))
            throw new AuthException("UserId cannot be empty");

        if (string.IsNullOrEmpty(tokenHash))
            throw new AuthException("Token hash cannot be empty");

        Id = new Guid();
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = DateTime.UtcNow.AddDays(expiryDays);
        IsRevoked = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Revoke()
    {
        if (IsRevoked)
            throw new AuthException("Token already revoked");

        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }

    public bool IsValid()
    {
        return !IsRevoked && !IsExpired();
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }

}