namespace ECommerce.Products.Exceptions;

using BuildingBlocks.Exception;

public class InvalidBarcodeException : BadRequestException
{
    public InvalidBarcodeException(string barcode)
        : base($"Barcode: '{barcode}' is invalid.")
    {
    }
}