namespace ECommerce.Products.Exceptions;

using BuildingBlocks.Exception;

public class InvalidProductIdExceptions : BadRequestException
{
    public InvalidProductIdExceptions(Guid productId)
        : base($"ProductId: '{productId}' is invalid.")
    {
    }
}