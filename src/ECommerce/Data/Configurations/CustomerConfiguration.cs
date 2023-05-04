namespace ECommerce.Data.Configurations;

using Customers.Models;
using Customers.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerConfiguration :IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(nameof(Customer));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(customerId => customerId.Value, dbId => CustomerId.Of(dbId));

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.Name,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Customer.Name))
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.Address,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Customer.Address))
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.Mobile,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Customer.Mobile))
                    .IsRequired();
            }
        );
    }
}
