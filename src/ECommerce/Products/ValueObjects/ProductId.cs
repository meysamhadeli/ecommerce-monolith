namespace ECommerce.Products.ValueObjects;

using Exceptions;

public record ProductId
{
    public Guid Value { get; }

    private ProductId(Guid value)
    {
        Value = value;
    }

    public static ProductId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidProductIdExceptions(value);
        }

        return new ProductId(value);
    }
    public static implicit operator Guid(ProductId productId) => productId.Value;
}
