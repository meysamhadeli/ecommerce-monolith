namespace ECommerce.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Models;
using Products.ValueObjects;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(productId => productId.Value, dbId => ProductId.Of(dbId));


        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.Name,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.Name))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.Barcode,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.Barcode))
                    .HasMaxLength(20)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.Description,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.Description))
                    .HasMaxLength(200)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.Price,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.Price))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.ProfitMargin,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.ProfitMargin))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.NetPrice,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.NetPrice))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            }
        );

        builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
    }
}
