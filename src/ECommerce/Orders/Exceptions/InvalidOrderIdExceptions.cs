namespace ECommerce.Orders.Exceptions;

using BuildingBlocks.Exception;

public class InvalidOrderIdExceptions : BadRequestException
{
    public InvalidOrderIdExceptions(Guid orderId)
        : base($"OrderId: '{orderId}' is invalid.")
    {
    }
}
