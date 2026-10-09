namespace ECommerce.Orders.Exceptions;

using Griffin.Core.Exception;

public class OrderItemNotExistInInventoryException : NotFoundException
{
    public OrderItemNotExistInInventoryException(Guid productId, int? code = null) : base($"ProductId: {productId} not exist in inventory!", code)
    {
    }
}
