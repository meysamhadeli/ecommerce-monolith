namespace ECommerce.Inventories.Models;

using BuildingBlocks.Core.Event;
using BuildingBlocks.Core.Model;
using ValueObjects;

public record Inventory: Aggregate<InventoryId>
{
    public Name Name { get; private set; }

    public static Inventory Create(InventoryId id, Name name, bool isDeleted = false)
    {
        var inventory = new Inventory
        {
            Id = id,
            Name = name,
            IsDeleted = isDeleted
        };

        var @event = new InventoryCreatedDomainEvent(inventory.Id.Value, inventory.Name.Value, inventory.IsDeleted);

        inventory.AddDomainEvent(@event);

        return inventory;
    }
}

public record InventoryCreatedDomainEvent(Guid Id, string Name, bool IsDeleted) : IDomainEvent;
