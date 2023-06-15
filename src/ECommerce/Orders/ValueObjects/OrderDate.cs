namespace ECommerce.Orders.ValueObjects;

using Exceptions;

public record OrderDate
{
    public DateTime Value { get; }

    private OrderDate(DateTime value)
    {
        Value = value;
    }

    public static OrderDate Of(DateTime value)
    {
        if (value.Hour < 8 && value.Hour > 19)
        {
            throw new InvalidDateTimeRangeException();
        }

        return new OrderDate(value);
    }
    public static implicit operator DateTime(OrderDate orderDate) => orderDate.Value;
}

