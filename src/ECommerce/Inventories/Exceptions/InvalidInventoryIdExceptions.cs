namespace ECommerce.Inventories.Exceptions;

using BuildingBlocks.Exception;

public class InvalidInventoryIdExceptions : BadRequestException
{
    public InvalidInventoryIdExceptions(Guid inventoryId)
        : base($"InventoryId: '{inventoryId}' is invalid.")
    {
    }
}
