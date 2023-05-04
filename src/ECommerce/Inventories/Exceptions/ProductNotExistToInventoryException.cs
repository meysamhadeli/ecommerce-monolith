namespace ECommerce.Inventories.Exceptions;

using BuildingBlocks.Exception;

public class ProductNotExistToInventoryException : NotFoundException
{
    public ProductNotExistToInventoryException(int? code = default) : base("Product Not Exist in Inventory!", code)
    {
    }
}
