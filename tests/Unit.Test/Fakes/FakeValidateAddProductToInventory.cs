namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Inventories.Features.AddingProductToInventory;

public class FakeValidateAddProductToInventory : AutoFaker<AddProductToInventory>
{
    public FakeValidateAddProductToInventory()
    {
        RuleFor(r => r.ProductId, _ => Guid.Empty);
        RuleFor(r => r.InventoryId, _ => Guid.Empty);
        RuleFor(r => r.Quantity, _ => 0);
    }
}
