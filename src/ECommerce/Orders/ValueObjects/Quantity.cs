namespace ECommerce.Orders.ValueObjects;

using Exceptions;

public record Quantity
{
    public int Value { get; set; }

    private Quantity(int value)
    {
        if (value < 0)
            throw new InvalidQuantityException(value);

        Value = value;
    }

    public static Quantity Of(int value)
    {
        return new Quantity(value);
    }
}
