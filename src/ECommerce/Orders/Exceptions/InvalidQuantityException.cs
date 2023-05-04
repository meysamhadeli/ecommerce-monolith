namespace ECommerce.Orders.Exceptions;

using BuildingBlocks.Exception;

public class InvalidQuantityException : BadRequestException
{
    public InvalidQuantityException(int quantity)
        : base($"Quantity: '{quantity}' must be greater than 0.")
    {
    }
}
