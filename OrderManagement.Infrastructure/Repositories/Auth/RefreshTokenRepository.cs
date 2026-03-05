using System.Data;
using Dapper;
using OrderManagement.Application.Interfaces.Auth;
using OrderManagement.Domain.Entities.Auth;

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

    // public Task AddAsync(RefreshToken refreshToken)
    // {

    // }

    // public Task UpdateAsync(RefreshToken refreshToken)
    // {

    // }

    // public Task RevokeAllForUserAsync(string userId)
    // {

    // }
}