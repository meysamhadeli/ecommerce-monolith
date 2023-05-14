namespace ECommerce.Orders.ValueObjects;

using Exceptions;

public record TotalPrice
{
    public decimal Value { get;}

    private TotalPrice(decimal value)
    {
        if (value < 0)
            throw new InvalidTotalPriceException(value);

        Value = value;
    }

    public static TotalPrice Of(decimal value)
    {
        return new TotalPrice(value);
    }

    public static implicit operator decimal(TotalPrice totalPrice) => totalPrice?.Value ?? 0;

    public static TotalPrice operator +(TotalPrice price1, TotalPrice price2)
    {
        var sum = price1.Value + price2.Value;
        return new TotalPrice(sum);
    }

    public static TotalPrice operator -(TotalPrice price1, TotalPrice price2)
    {
        var subtract = price1.Value - price2.Value;
        return new TotalPrice(subtract);
    }
}

