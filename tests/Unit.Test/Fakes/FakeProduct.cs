namespace Unit.Test.Fakes;

using ECommerce.Categories.ValueObjects;
using ECommerce.Products.Models;
using ECommerce.Products.ValueObjects;
using MassTransit;
using Name = ECommerce.Products.ValueObjects.Name;

public sealed class FakeProduct
{
    public static Product Generate()
    {
        return Product.Create(ProductId.Of(NewId.NextGuid()), Name.Of("Coffee"), Barcode.Of("129982882"), true,
            CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")), Price.Of(50000), ProfitMargin.Of(0),
            Description.Of("this is a coffee"));
    }
}
