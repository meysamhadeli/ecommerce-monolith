namespace ECommerce.Products.Exceptions;

using Griffin.Core.Exception;

public class InvalidProductIdExceptions : BadRequestException
{
    public InvalidProductIdExceptions(Guid productId)
        : base($"ProductId: '{productId}' is invalid.")
    {
    }
}