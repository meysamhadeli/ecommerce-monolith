namespace ECommerce.Orders.Exceptions;

using BuildingBlocks.Exception;

public class OrderItemNotExistInInventoryException : NotFoundException
{
    public OrderItemNotExistInInventoryException(Guid productId, int? code = null) : base($"ProductId: {productId} not exist in inventory!", code)
    {
    }
}
