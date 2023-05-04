namespace ECommerce.Products.Exceptions;

using BuildingBlocks.Exception;

public class LongLengthBarcodeException : BadRequestException
{
    public LongLengthBarcodeException(string barcode, int maxLength)
        : base($"Barcode: '{barcode}' cannot be longer than {maxLength} characters")
    {
    }
}