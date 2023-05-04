namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Inventories.Features.DamagingProduct;

public class FakeValidateDamageProduct : AutoFaker<DamageProduct>
{
    public FakeValidateDamageProduct()
    {
        RuleFor(r => r.ProductId, _ => Guid.Empty);
        RuleFor(r => r.Quantity, _ => 0);
    }
}

