namespace ECommerce.Customers.ValueObjects;

using Exceptions;

public record CustomerId
{
    public Guid Value { get; }

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

    public static implicit operator Guid(CustomerId customerId) => customerId.Value;
}
