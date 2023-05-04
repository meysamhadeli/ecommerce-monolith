using CategoryId = ECommerce.Categories.ValueObjects.CategoryId;

namespace ECommerce.Data.Configurations;

using Categories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(nameof(Category));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(categoryId => categoryId.Value, dbId => CategoryId.Of(dbId));

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.Name,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Category.Name))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );
    }
}
