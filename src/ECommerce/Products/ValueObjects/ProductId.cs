namespace ECommerce.Products.ValueObjects;

using Exceptions;

public record ProductId
{
    public Guid Value { get; set; }

    private ProductId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidProductIdExceptions(value);

        Value = value;
    }

    public static ProductId Of(Guid value)
    {
        return new ProductId(value);
    }
    public static implicit operator Guid(ProductId productId) => productId.Value;
}
