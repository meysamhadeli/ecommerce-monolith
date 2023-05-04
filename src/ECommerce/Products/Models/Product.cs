namespace ECommerce.Products.Models;

using BuildingBlocks.Core.Model;
using Categories.Models;
using Categories.ValueObjects;
using Features.CreatingProduct;
using JetBrains.Annotations;
using ValueObjects;
using Name = ValueObjects.Name;

public record Product : Aggregate<ProductId>
{
    private NetPrice _netPrice;
    public Name Name { get; private set; }
    public Barcode Barcode { get; private set; }
    public Description? Description { get; private set; }
    public Category? Category { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public bool IsBreakable { get; private set; }
    public Price Price { get; set; }

    public NetPrice? NetPrice
    {
        get
        {
            return _netPrice;
        }
        set
        {
           _netPrice = value;
        }
    }

    public ProfitMargin? ProfitMargin { get; set; }

    public static Product Create(ProductId id, Name name, Barcode barcode, bool isBreakable,
        CategoryId categoryId,
        Price price,
        ProfitMargin profitMargin,
        Description? description = null, bool isDeleted = false)
    {
        var product = new Product
        {
            Id = id,
            Name = name,
            Barcode = barcode,
            IsBreakable = isBreakable,
            CategoryId = categoryId,
            Description = description,
            Price = price,
            ProfitMargin = profitMargin,
            IsDeleted = isDeleted
        };

        var @event = new ProductCreatedDomainEvent(product.Id, product.Name, product.Barcode,
            product.IsBreakable, product.CategoryId, product.Price, product.ProfitMargin, product.NetPrice,
            product.Description, product.IsDeleted);

        product.AddDomainEvent(@event);

        return product;
    }
}
