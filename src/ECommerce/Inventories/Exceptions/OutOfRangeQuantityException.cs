namespace ECommerce.Inventories.Exceptions;

using BuildingBlocks.Exception;

public class OutOfRangeQuantityException : BadRequestException
{
    public OutOfRangeQuantityException(int quantity, int quantityInventory, int? code = default) : base($"The Quantity: {quantity} that requested is more than our inventory with Quantity: {quantityInventory}!", code)
    {
    }
}

