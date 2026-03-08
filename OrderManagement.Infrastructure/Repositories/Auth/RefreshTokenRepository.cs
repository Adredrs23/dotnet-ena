namespace OrderManagement.Infrastructure.Repositories.Auth;

using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces.Auth;
using OrderManagement.Domain.Entities.Auth;
using OrderManagement.Infrastructure.Persistence.Auth;

public class RefreshTokenRepository : IRefreshTokenRepository
{


    private readonly IDbConnection _connection;

    private readonly AuthDbContext _authDbContext;

    public RefreshTokenRepository(IDbConnection connection, AuthDbContext authDbContext)
    {
        _connection = connection;
        _authDbContext = authDbContext;
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
    {
        const string sql = @"
            SELECT Id, UserId, TokenHash, ExpiresAt, IsRevoked, CreatedAt, RevokedAt
            FROM RefreshTokens
            WHERE TokenHash = @TokenHash
            LIMIT 1";

        return await _connection.QuerySingleOrDefaultAsync<RefreshToken>(sql, new { tokenHash });
    }

    public async Task<RefreshToken?> GetActiveByUserIdAsync(string userId)
    {
        const string sql = @"
            SELECT Id, UserId, TokenHash, ExpiresAt, IsRevoked, CreatedAt, RevokedAt
            FROM RefreshTokens
            WHERE UserId = @UserId 
              AND IsRevoked = 0
              AND ExpiresAt > @Now
            ORDER BY CreatedAt DESC
            LIMIT 1";

        return await _connection.QuerySingleOrDefaultAsync<RefreshToken>(
            sql,
            new { UserId = userId, Now = DateTime.UtcNow }
        );

    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _authDbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _authDbContext.RefreshTokens.Update(refreshToken);
        // await _authDbContext.SaveChangesAsync();
    }

    public async Task RevokeAllForUserAsync(string userId)
    {
        // Load all active tokens for user
        var tokens = await _authDbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        // Revoke each one (domain logic)
        foreach (var token in tokens)
        {
            token.Revoke();
        }

        // await _authDbContext.SaveChangesAsync();
    }

    public async Task DeleteExpiredTokensAsync()
    {
        var expiredTokens = await _authDbContext.RefreshTokens
            .Where(rt => rt.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        _authDbContext.RefreshTokens.RemoveRange(expiredTokens);
        // await _authDbContext.SaveChangesAsync();
    }
}