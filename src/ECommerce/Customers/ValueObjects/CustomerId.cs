namespace ECommerce.Customers.ValueObjects;

using Exceptions;

public record CustomerId
{
    public Guid Value { get; set; }

    private CustomerId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidCustomerIdExceptions(value);

        Value = value;
    }

    public static CustomerId Of(Guid value)
    {
        return new CustomerId(value);
    }
}
