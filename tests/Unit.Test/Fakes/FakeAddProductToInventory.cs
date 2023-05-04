namespace Unit.Test.Fakes;

using ECommerce.Inventories.Models;
using ECommerce.Inventories.ValueObjects;
using ECommerce.Products.ValueObjects;
using MassTransit;

public class FakeAddProductToInventory
{
    public static InventoryItems Generate()
    {
        return InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
            InventoryId.Of(NewId.NextGuid()), ProductId.Of(NewId.NextGuid()), Quantity.Of(2));
    }
}
