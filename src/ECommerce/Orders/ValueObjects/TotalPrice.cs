namespace ECommerce.Orders.ValueObjects;

using Exceptions;

public record TotalPrice
{
    public decimal Value { get; set; }

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
}

