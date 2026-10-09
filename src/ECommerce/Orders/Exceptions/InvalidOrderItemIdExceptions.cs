namespace ECommerce.Orders.Exceptions;

using Griffin.Core.Exception;

public class InvalidOrderItemIdExceptions : BadRequestException
{
    public InvalidOrderItemIdExceptions(Guid orderItemId)
        : base($"OrderItemId: '{orderItemId}' is invalid.")
    {
    }
}

