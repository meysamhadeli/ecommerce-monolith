namespace ECommerce.Orders.Exceptions;

using BuildingBlocks.Exception;

public class InvalidTotalPriceException : BadRequestException
{
    public InvalidTotalPriceException(decimal totalPrice)
        : base($"TotalPrice: '{totalPrice}' must be 0 or grater than.")
    {
    }
}
