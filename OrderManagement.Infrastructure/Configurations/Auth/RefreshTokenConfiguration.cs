namespace OrderManagement.Infrastructure.Configurations.Auth;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entities.Auth;
using OrderManagement.Infrastructure.Identity;

public class RefrestTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{

    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.UserId)
                          .IsRequired()
                          .HasMaxLength(450);

        builder.Property(rt => rt.TokenHash)
        .IsRequired()
        .HasMaxLength(256);

        builder.Property(rt => rt.ExpiresAt).IsRequired();

        builder.Property(rt => rt.IsRevoked).IsRequired().HasDefaultValue(false);

        builder.Property(rt => rt.CreatedAt).IsRequired();


        builder.HasIndex(rt => rt.TokenHash)
                          .IsUnique();

        builder.HasIndex(rt => rt.UserId);


        builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(rt => rt.UserId).OnDelete(DeleteBehavior.Cascade);


    }
}

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new IdentityRole
            {
                Id = "2",
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        );
    }
}