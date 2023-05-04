namespace Integration.Test.Fakes;

using AutoBogus;
using ECommerce.Inventories.Features.SellingProduct;

public sealed class FakeSellProductCommand : AutoFaker<SellProduct>
{
    public FakeSellProductCommand()
    {
        RuleFor(r => r.ProductId, r => new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3"));
        RuleFor(r => r.Quantity, _ => 4);
    }
}

