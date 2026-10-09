namespace ECommerce.Products.Exceptions;

using Griffin.Core.Exception;

public class InvalidBarcodeException : BadRequestException
{
    public InvalidBarcodeException(string barcode)
        : base($"Barcode: '{barcode}' is invalid.")
    {
    }
}