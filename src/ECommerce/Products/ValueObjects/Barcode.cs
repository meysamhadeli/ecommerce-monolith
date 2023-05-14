namespace ECommerce.Products.ValueObjects;

using Exceptions;

public record Barcode
{
    private const int MaxLength = 20;
    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }

    private Barcode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidBarcodeException(value);

        if (value.Length > MaxLength)
            throw new LongLengthBarcodeException(value, MaxLength);

        Value = value;
    }

    public static Barcode Of(string value)
    {
        return new Barcode(value);
    }

    public static implicit operator string(Barcode barcode) => barcode.Value;
}
