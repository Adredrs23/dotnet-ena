namespace OrderManagement.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;


public class ApplicationUser : IdentityUser
{
    // IdentityUser already has:
    // - Id (string)
    // - Email
    // - EmailConfirmed
    // - PasswordHash
    // - SecurityStamp
    // - PhoneNumber
    // - TwoFactorEnabled
    // - LockoutEnd
    // - AccessFailedCount

    // Add your custom properties:
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // Navigation property to orders
    // public ICollection<Order> Orders { get; set; } = new List<Order>();
}