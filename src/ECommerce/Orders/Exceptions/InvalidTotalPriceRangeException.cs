namespace ECommerce.Orders.Exceptions;

using BuildingBlocks.Exception;

public class InvalidTotalPriceRangeException : BadRequestException
{
    public InvalidTotalPriceRangeException(decimal totalPrice)
        : base($"TotalPrice: '{totalPrice}' must be grater than 50000.")
    {
    }
}
