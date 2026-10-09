namespace ECommerce.Inventories.Exceptions;

using Griffin.Core.Exception;

public class InvalidInventoryIdExceptions : BadRequestException
{
    public InvalidInventoryIdExceptions(Guid inventoryId)
        : base($"InventoryId: '{inventoryId}' is invalid.")
    {
    }
}
