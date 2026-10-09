namespace ECommerce.Inventories.Exceptions;

using Griffin.Core.Exception;

public class ProductNotExistToInventoryException : NotFoundException
{
    public ProductNotExistToInventoryException(int? code = default) : base("Product Not Exist in Inventory!", code)
    {
    }
}
