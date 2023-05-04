namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Inventories.Features.AddingProductToInventory;
using MassTransit;

public sealed class FakeAddProductToInventoryCommand : AutoFaker<AddProductToInventory>
{
    public FakeAddProductToInventoryCommand()
    {
        RuleFor(r => r.Id, _ => NewId.NextGuid());
        RuleFor(r => r.InventoryId, r => new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4"));
        RuleFor(r => r.ProductId, r => new Guid("1c5c0000-97c6-fc34-fcd3-08db322230c0"));
        RuleFor(r => r.Quantity, _ => 5);
    }
}
