namespace Integration.Test.Fakes;

using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Models;
using MassTransit;

public class FakeAddProductToInventory
{
    public static InventoryItems Generate(ProductStatus status)
    {
        return new InventoryItems
        {
            Id = NewId.NextGuid(),
            InventoryId = new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4"),
            ProductId = new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0"),
            Quantity = 2,
            Status = status,
        };
    }
}
