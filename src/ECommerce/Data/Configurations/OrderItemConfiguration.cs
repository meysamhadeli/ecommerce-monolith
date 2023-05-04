namespace ECommerce.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Models;
using Orders.ValueObjects;

public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(nameof(OrderItem));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(orderItemId => orderItemId.Value, dbId => OrderItemId.Of(dbId));

        builder.Property(r => r.IsDeleted);

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.Quantity,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(OrderItem.Quantity))
                    .IsRequired();
            }
        );

        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        builder.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId);
    }
}
