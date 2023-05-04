namespace ECommerce.Inventories.Exceptions;

using BuildingBlocks.Exception;

public class InvalidInventoryItemsIdExceptions : BadRequestException
{
    public InvalidInventoryItemsIdExceptions(Guid inventoryItemsId)
        : base($"InventoryItems: '{inventoryItemsId}' is invalid.")
    {
    }
}
