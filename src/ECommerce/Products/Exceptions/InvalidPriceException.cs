namespace ECommerce.Products.Exceptions;

using Griffin.Core.Exception;

public class InvalidPriceException : BadRequestException
{
    public InvalidPriceException(decimal price)
        : base($"Price: '{price}' must be grater than 0.")
    {
    }
}
