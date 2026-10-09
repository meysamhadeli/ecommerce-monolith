namespace ECommerce.Data.Configurations;

using Inventories.Enums;
using Inventories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InventoryItemsConfigurations : IEntityTypeConfiguration<InventoryItems>
{
    public void Configure(EntityTypeBuilder<InventoryItems> builder)
    {
        builder.ToTable(nameof(InventoryItems));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(ProductStatus.InStock)
            .HasConversion(
                x => x.ToString(),
                x => (ProductStatus)Enum.Parse(typeof(ProductStatus), x));

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        builder.HasOne(x => x.Inventory).WithMany().HasForeignKey(x => x.InventoryId);
    }
}
