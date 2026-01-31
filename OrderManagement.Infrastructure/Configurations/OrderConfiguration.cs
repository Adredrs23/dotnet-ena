namespace OrderManagement.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entities;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{

    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.CreatedAt).IsRequired();

        builder.OwnsMany(o => o.Items, itemBuilder =>
        {
            itemBuilder.WithOwner().HasForeignKey("OrderId");
            itemBuilder.HasKey("Id");
        });
    }
}