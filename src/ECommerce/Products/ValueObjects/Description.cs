namespace ECommerce.Products.ValueObjects;

using Exceptions;

public record Description
{
    private const int MaxLength = 200;
    private const int MinLength = 5;

    public string Value { get; set; }

    public override string ToString()
    {
        return Value;
    }

    private Description(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidateNullOrEmptyDescriptionException(value);

        if (value.Length < MinLength)
            throw new ShortLengthDescriptionException(value, MinLength);

        if (value.Length > MaxLength)
            throw new LongLengthDescriptionException(value, MaxLength);

        Value = value;
    }

    public static Description Of(string value)
    {
        return new Description(value);
    }

    public static implicit operator string(Description description) => description.Value;
}
