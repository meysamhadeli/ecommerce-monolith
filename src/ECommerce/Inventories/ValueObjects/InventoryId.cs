namespace ECommerce.Inventories.ValueObjects;

using Exceptions;

public record InventoryId
{
    public Guid Value { get; }

    private InventoryId(Guid value)
    {
        Value = value;
    }

    public static InventoryId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidInventoryIdExceptions(value);
        }

        return new InventoryId(value);
    }

    public static implicit operator Guid(InventoryId inventoryId) => inventoryId.Value;
}
