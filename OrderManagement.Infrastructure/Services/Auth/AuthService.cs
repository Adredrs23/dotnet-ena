namespace OrderManagement.Application.Services.Auth;

using Microsoft.AspNetCore.Identity;
using OrderManagement.Application.Commands.Auth;
using OrderManagement.Application.Interfaces.Auth;
using OrderManagement.Application.Queries.Auth;
using OrderManagement.Domain.Entities.Auth;
using OrderManagement.Domain.Exceptions.Auth;
using OrderManagement.Infrastructure.Identity;



public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IGoogleAuthService _googleAuthService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IGoogleAuthService googleAuthService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _googleAuthService = googleAuthService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterCommand command)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(command.Email);
        if (existingUser != null)
        {
            throw new UserAlreadyExistsException(command.Email);
        }

        // Create new user
        var user = new ApplicationUser
        {
            UserName = command.Email,
            Email = command.Email,
            EmailConfirmed = true // For simplicity, auto-confirm (in production, send email)
        };

        // Create user with password (Identity handles hashing!)
        var result = await _userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new AuthException($"User registration failed: {errors}");
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, "User");

        // Generate tokens
        return await GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginCommand command)
    {
        // Find user by email
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        // Check password
        var result = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                throw new AuthException("Account is locked due to multiple failed login attempts");
            }

            throw new InvalidCredentialsException();
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // Generate tokens
        return await GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenCommand command)
    {
        // Hash the incoming refresh token
        var tokenHash = _tokenService.HashToken(command.RefreshToken);

        // Find token in database
        var refreshToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (refreshToken == null)
        {
            throw new AuthException("Invalid refresh token");
        }

        // Validate token
        if (!refreshToken.IsValid())
        {
            if (refreshToken.IsRevoked)
            {
                // Token reuse detected! Possible attack
                // Revoke ALL tokens for this user
                await _refreshTokenRepository.RevokeAllForUserAsync(refreshToken.UserId);
                throw new TokenRevokedException();
            }

            if (refreshToken.IsExpired())
            {
                throw new TokenExpiredException();
            }

            throw new AuthException("Invalid refresh token");
        }

        // Get user
        var user = await _userManager.FindByIdAsync(refreshToken.UserId);
        if (user == null)
        {
            throw new AuthException("User not found");
        }

        // IMPORTANT: Revoke old token (rotation!)
        refreshToken.Revoke();
        await _refreshTokenRepository.UpdateAsync(refreshToken);

        // Generate NEW tokens
        return await GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginCommand command)
    {
        // Validate Google token
        var googleInfo = await _googleAuthService.ValidateGoogleTokenAsync(command.GoogleIdToken);

        // Find or create user
        var user = await _userManager.FindByEmailAsync(googleInfo.Email);

        if (user == null)
        {
            // Create new user
            user = new ApplicationUser
            {
                UserName = googleInfo.Email,
                Email = googleInfo.Email,
                EmailConfirmed = true, // Google already verified email
                FirstName = googleInfo.Name.Split(' ').FirstOrDefault(),
                LastName = googleInfo.Name.Split(' ').Skip(1).FirstOrDefault(),
                ProfilePictureUrl = googleInfo.Picture
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new AuthException($"Failed to create user from Google login: {errors}");
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");
        }

        // Link external login (Google)
        var loginInfo = new UserLoginInfo("Google", googleInfo.GoogleUserId, "Google");
        var existingLogin = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);

        if (existingLogin == null)
        {
            // Link Google account to user
            await _userManager.AddLoginAsync(user, loginInfo);
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // Generate tokens
        return await GenerateAuthResponse(user);
    }

    public async Task LogoutAsync(LogoutCommand command)
    {
        // Hash the token
        var tokenHash = _tokenService.HashToken(command.RefreshToken);

        // Find token
        var refreshToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (refreshToken != null && !refreshToken.IsRevoked)
        {
            // Revoke it
            refreshToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(refreshToken);
        }

        // Note: Access tokens can't be revoked (stateless)
        // They'll expire naturally after 15 minutes
    }

    public async Task RevokeAllTokensAsync(string userId)
    {
        await _refreshTokenRepository.RevokeAllForUserAsync(userId);
    }

    // ═════════════════════════════════════════════════════════════
    // PRIVATE HELPER METHODS
    // ═════════════════════════════════════════════════════════════

    private async Task<AuthResponseDto> GenerateAuthResponse(ApplicationUser user)
    {
        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);

        // Generate access token (JWT)
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email!, roles);

        // Generate refresh token (random)
        var refreshTokenValue = _tokenService.GenerateRefreshToken();

        // Hash refresh token before storing
        var refreshTokenHash = _tokenService.HashToken(refreshTokenValue);

        // Create refresh token entity
        var refreshToken = new RefreshToken(user.Id, refreshTokenHash, expiryDays: 7);

        // Store refresh token
        await _refreshTokenRepository.AddAsync(refreshToken);

        // Build response
        return new AuthResponseDto(
            AccessToken: accessToken,
            RefreshToken: refreshTokenValue, // Send raw token to client!
            ExpiresAt: DateTime.UtcNow.AddMinutes(15),
            User: new UserDto(
                Id: user.Id,
                Email: user.Email!,
                Roles: roles.ToArray()
            )
        );
    }
}