namespace ECommerce.Products.ValueObjects;

using Exceptions;

public record Price
{
    public decimal Value { get; }

    private Price(decimal value)
    {
        Value = value;
    }

    public static Price Of(decimal value)
    {
        if (value <= 0)
        {
            throw new InvalidPriceException(value);
        }

        return new Price(value);
    }

    public static implicit operator decimal(Price price) => price.Value;
}

