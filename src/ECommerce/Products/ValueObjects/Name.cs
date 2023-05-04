namespace ECommerce.Products.ValueObjects;

using Exceptions;

public record Name
{
    private const int MaxLength = 50;

    private const int MinLength = 2;
    public string Value { get; set; }

    private Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidNullOrEmptyNameException(value);

        if (value.Length < MinLength)
            throw new ShortLengthNameException(value, MinLength);

        if (value.Length > MaxLength)
            throw new LongLengthNameException(value, MaxLength);

        Value = value;
    }

    public static Name Of(string value)
    {
        return new Name(value);
    }
    public static implicit operator string(Name name) => name.Value;
}
