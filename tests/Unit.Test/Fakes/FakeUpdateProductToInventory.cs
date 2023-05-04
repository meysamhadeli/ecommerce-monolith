namespace Unit.Test.Fakes;

using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Models;
using ECommerce.Inventories.ValueObjects;
using ECommerce.Products.ValueObjects;
using MassTransit;

public class FakeUpdateProductToInventory
{
    public static InventoryItems Generate(int newQuantity, ProductStatus status)
    {
        var inventoryItem = InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
            InventoryId.Of(NewId.NextGuid()), ProductId.Of(NewId.NextGuid()), Quantity.Of(2));

        inventoryItem.UpdateProductToInventory(inventoryItem.Id, inventoryItem.InventoryId, inventoryItem.ProductId,
            Quantity.Of(newQuantity), status);

        return inventoryItem;
    }
}

