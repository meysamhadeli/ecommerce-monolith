namespace Unit.Test.Fakes;

using ECommerce.Inventories.Models;
using ECommerce.Inventories.ValueObjects;
using MassTransit;

public sealed class FakeInventory
{
    public static Inventory Generate()
    {
        return Inventory.Create(InventoryId.Of(NewId.NextGuid()),Name.Of("Central-Inventory"));
    }
}

