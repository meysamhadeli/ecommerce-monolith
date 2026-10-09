namespace ECommerce.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Enums;
using Orders.Models;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(OrderStatus.Pending)
            .HasConversion(
                x => x.ToString(),
                x => (OrderStatus)Enum.Parse(typeof(OrderStatus), x));

        builder.Property(x => x.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.OrderDate)
            .IsRequired();

        builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId);

        builder.HasMany(x => x.OrderItems).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
    }
}
