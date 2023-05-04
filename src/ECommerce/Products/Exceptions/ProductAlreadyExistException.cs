namespace ECommerce.Products.Exceptions;

using BuildingBlocks.Exception;

public class ProductAlreadyExistException : ConflictException
{
    public ProductAlreadyExistException(int? code = default) : base("Product already exist!", code)
    {
    }
}
