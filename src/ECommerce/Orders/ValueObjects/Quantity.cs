namespace ECommerce.Orders.ValueObjects;

using Exceptions;

public record Quantity
{
    public int Value { get; }

    private Quantity(int value)
    {
        if (value < 1)
            throw new InvalidQuantityException(value);

        Value = value;
    }

    public static Quantity Of(int value)
    {
        return new Quantity(value);
    }

    public static implicit operator int(Quantity quantity) => quantity?.Value ?? 0;
}
