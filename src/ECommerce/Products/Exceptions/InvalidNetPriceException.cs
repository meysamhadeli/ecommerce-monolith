namespace ECommerce.Products.Exceptions;

using BuildingBlocks.Exception;

public class InvalidNetPriceException : BadRequestException
{
    public InvalidNetPriceException(decimal netPrice)
        : base($"NetPrice: '{netPrice}' must be grater than 0.")
    {
    }
}
