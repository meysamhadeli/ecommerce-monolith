namespace ECommerce.Products.ValueObjects;

using Exceptions;

public record NetPrice
{
    public decimal Value { get; }

    private NetPrice(decimal value)
    {
        Value = value;
    }

    public static NetPrice Of(decimal value)
    {
        if (value < 0)
        {
            throw new InvalidNetPriceException(value);
        }

        return new NetPrice(value);
    }

    public static implicit operator decimal(NetPrice netPrice) => netPrice?.Value ?? 0;
}


