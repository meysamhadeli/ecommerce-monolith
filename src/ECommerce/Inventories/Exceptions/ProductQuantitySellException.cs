namespace ECommerce.Inventories.Exceptions;

using BuildingBlocks.Exception;

public class ProductQuantityException : BadRequestException
{
    public ProductQuantityException(int? code = default) : base("The quantity of product is 0!", code)
    {
    }
}

