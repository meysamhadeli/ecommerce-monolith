namespace ECommerce.Inventories.Exceptions;

using Griffin.Core.Exception;

public class InvalidQuantityException : BadRequestException
{
    public InvalidQuantityException(int quantity)
        : base($"Quantity: '{quantity}' must be equal or greater than 0.")
    {
    }
}

