namespace ECommerce.Orders.Exceptions;

using Griffin.Core.Exception;

public class InvalidOrderIdExceptions : BadRequestException
{
    public InvalidOrderIdExceptions(Guid orderId)
        : base($"OrderId: '{orderId}' is invalid.")
    {
    }
}
