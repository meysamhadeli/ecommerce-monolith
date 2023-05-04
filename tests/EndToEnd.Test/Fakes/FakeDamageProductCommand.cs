namespace EndToEnd.Test.Fakes;

using AutoBogus;
using ECommerce.Inventories.Features.DamagingProduct;

public sealed class FakeDamageProductCommand : AutoFaker<DamageProduct>
{
    public FakeDamageProductCommand()
    {
        RuleFor(r => r.ProductId, r => new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c2"));
        RuleFor(r => r.Quantity, _ => 5);
    }
}

