namespace ECommerce.Inventories.ValueObjects;

using Exceptions;
using Products.Exceptions;

public record InventoryItemsId
{
    public Guid Value { get; }

    private InventoryItemsId(Guid value)
    {
        Value = value;
    }

    public static InventoryItemsId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidInventoryItemsIdExceptions(value);
        }

        return new InventoryItemsId(value);
    }

    public static implicit operator Guid(InventoryItemsId inventoryItemsId) => inventoryItemsId.Value;
}
