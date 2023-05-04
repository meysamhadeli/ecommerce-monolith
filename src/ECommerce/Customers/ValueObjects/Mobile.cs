namespace ECommerce.Customers.ValueObjects;

using System.Text.RegularExpressions;
using Exceptions;

public record Mobile
{
    public string Value { get; set; }

    public override string ToString()
    {
        return Value;
    }

    private Mobile(string value)
    {
        // Basic validation rule: phone number must be a 10-digit number
        if (Regex.IsMatch(value, @"^\d{10}$"))
        {
            throw new InvalidMobileException(value);
        }

        Value = value;
    }

    public static Mobile Of(string number)
    {
        return new Mobile(number);
    }

    public static implicit operator string(Mobile mobile) => mobile.Value;
}
