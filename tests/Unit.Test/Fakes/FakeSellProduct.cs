namespace Unit.Test.Fakes;

using ECommerce.Inventories.Models;
using ECommerce.Inventories.ValueObjects;
using ECommerce.Products.ValueObjects;
using MassTransit;

public class FakeSellProduct
{
    public static InventoryItems Generate(int InventoryProductQuantity, int soldQuantity)
    {
        var inventoryItem = InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
            InventoryId.Of(NewId.NextGuid()), ProductId.Of(NewId.NextGuid()), Quantity.Of(InventoryProductQuantity));

        inventoryItem.SellProduct(inventoryItem.Id, inventoryItem.InventoryId, inventoryItem.ProductId,
            Quantity.Of(soldQuantity));

        return inventoryItem;
    }
}
