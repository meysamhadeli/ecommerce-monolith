namespace ECommerce.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Enums;
using Orders.Models;
using Orders.ValueObjects;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(orderId => orderId.Value, dbId => OrderId.Of(dbId));

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(OrderStatus.Pending)
            .HasConversion(
                x => x.ToString(),
                x => (OrderStatus)Enum.Parse(typeof(OrderStatus), x));

        builder.OwnsOne(
            x => x.TotalPrice,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Order.TotalPrice))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.OrderDate,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Order.OrderDate))
                    .IsRequired();
            }
        );

        builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId);
    }
}
