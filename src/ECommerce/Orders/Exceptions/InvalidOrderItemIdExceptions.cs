namespace ECommerce.Orders.Exceptions;

using BuildingBlocks.Exception;

public class InvalidOrderItemIdExceptions : BadRequestException
{
    public InvalidOrderItemIdExceptions(Guid orderItemId)
        : base($"OrderItemId: '{orderItemId}' is invalid.")
    {
    }
}

