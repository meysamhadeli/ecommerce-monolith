namespace ECommerce.Data.Configurations;

using Inventories.Models;
using Inventories.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable(nameof(Inventory));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(inventoryId => inventoryId.Value, dbId => InventoryId.Of(dbId));

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.Name,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Inventory.Name))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );
    }
}

