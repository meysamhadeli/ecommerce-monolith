namespace ECommerce.Inventories.Exceptions;

using Griffin.Core.Exception;

public class InvalidInventoryItemsIdExceptions : BadRequestException
{
    public InvalidInventoryItemsIdExceptions(Guid inventoryItemsId)
        : base($"InventoryItems: '{inventoryItemsId}' is invalid.")
    {
    }
}
