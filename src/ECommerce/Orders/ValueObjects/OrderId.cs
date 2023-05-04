namespace ECommerce.Orders.ValueObjects;

using Exceptions;

public record OrderId
{
    public Guid Value { get; set; }

    private OrderId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidOrderIdExceptions(value);

        Value = value;
    }

    public static OrderId Of(Guid value)
    {
        return new OrderId(value);
    }
}
