namespace ECommerce.Inventories.ValueObjects;

using Exceptions;

public record InventoryId
{
    public Guid Value { get; set; }

    private InventoryId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidInventoryIdExceptions(value);

        Value = value;
    }

    public static InventoryId Of(Guid value)
    {
        return new InventoryId(value);
    }

    public static implicit operator Guid(InventoryId inventoryId) => inventoryId.Value;
}
