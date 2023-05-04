namespace ECommerce.Orders.Exceptions;

using BuildingBlocks.Exception;

public class InvalidTotalPriceException : BadRequestException
{
    public InvalidTotalPriceException(decimal totalPrice)
        : base($"TotalPrice: '{totalPrice}' must be equal or grater than 0.")
    {
    }
}
