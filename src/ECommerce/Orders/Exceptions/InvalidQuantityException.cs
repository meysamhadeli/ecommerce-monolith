namespace ECommerce.Orders.Exceptions;

using Griffin.Core.Exception;

public class InvalidQuantityException : BadRequestException
{
    public InvalidQuantityException(int quantity)
        : base($"Quantity: '{quantity}' must be greater than 0.")
    {
    }
}
