namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Inventories.Features.SellingProduct;

public class FakeValidateSellProduct : AutoFaker<SellProduct>
{
    public FakeValidateSellProduct()
    {
        RuleFor(r => r.ProductId, _ => Guid.Empty);
        RuleFor(r => r.Quantity, _ => 0);
    }
}

