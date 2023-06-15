namespace ECommerce.Products.ValueObjects;

using Exceptions;

public record ProfitMargin
{
    public decimal Value { get; }

    private ProfitMargin(decimal value)
    {
        Value = value;
    }

    public static ProfitMargin Of(decimal value)
    {
        if (value < 0)
        {
            throw new InvalidProfitMarginException(value);
        }

        return new ProfitMargin(value);
    }

    public static implicit operator decimal(ProfitMargin profitMargin) => profitMargin.Value;
}
