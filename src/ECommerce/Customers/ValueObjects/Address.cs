namespace ECommerce.Customers.ValueObjects;

using Exceptions;

public record Address
{
    public Address()
    {

    }

    public override string ToString()
    {
        return Value;
    }

    public string Value { get; }

    private Address(string street, string city, string state)
    {
        if (string.IsNullOrEmpty(street) &&
            string.IsNullOrEmpty(city) &&
            string.IsNullOrEmpty(state))
        {
            throw new InvalidNullOrEmptyAddressException();
        }

        Value = $"{street} - {city} - {state}";
    }

    public static Address Of(string street, string city, string state)
    {
        return new Address(street, city, state);
    }

    public static implicit operator string(Address address) => address.Value;
}
