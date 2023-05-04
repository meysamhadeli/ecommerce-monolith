namespace ECommerce.Orders.Exceptions;

using BuildingBlocks.Exception;

public class InvalidOrderQuantityException: BadRequestException
{
    public InvalidOrderQuantityException(int? code = null) : base("Order Quantity must be greater than 0.", code)
    {
    }
}
