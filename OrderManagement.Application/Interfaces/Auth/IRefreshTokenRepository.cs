namespace OrderManagement.Application.Interfaces.Auth;

using OrderManagement.Domain.Entities.Auth;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);

    public Task<RefreshToken?> GetActiveByUserIdAsync(string userId);

    public Task AddAsync(RefreshToken refreshToken);

    public Task UpdateAsync(RefreshToken refreshToken);

    public Task RevokeAllForUserAsync(string userId);
}