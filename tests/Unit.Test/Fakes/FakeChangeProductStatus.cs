namespace Unit.Test.Fakes;

using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Models;
using ECommerce.Inventories.ValueObjects;
using ECommerce.Products.ValueObjects;
using MassTransit;

public class FakeChangeProductStatus
{
    public static InventoryItems Generate(ProductStatus oldProductStatus, ProductStatus newProductStatus)
    {
        var inventoryItem = InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
            InventoryId.Of(NewId.NextGuid()), ProductId.Of(NewId.NextGuid()), Quantity.Of(2), oldProductStatus);

        inventoryItem.ChangeProductStatus(inventoryItem.Id, inventoryItem.InventoryId, inventoryItem.ProductId,
            inventoryItem.Quantity, newProductStatus);

        return inventoryItem;
    }
}
