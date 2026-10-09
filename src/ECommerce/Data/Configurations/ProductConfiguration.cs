namespace ECommerce.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Models;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Barcode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(200);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.ProfitMargin)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.NetPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
    }
}
