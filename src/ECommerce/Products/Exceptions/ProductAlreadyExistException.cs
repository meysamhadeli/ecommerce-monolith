namespace ECommerce.Products.Exceptions;

using Griffin.Core.Exception;

public class ProductAlreadyExistException : ConflictException
{
    public ProductAlreadyExistException(int? code = default) : base("Product already exist!", code)
    {
    }
}
