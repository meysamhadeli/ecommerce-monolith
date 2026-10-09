namespace Integration.Test.Fakes;

using ECommerce.Inventories.Models;
using MassTransit;

public sealed class FakeInventory
{
    public static Inventory Generate()
    {
        return new Inventory
        {
            Id = NewId.NextGuid(),
            Name = "Central-Inventory",
        };
    }
}
