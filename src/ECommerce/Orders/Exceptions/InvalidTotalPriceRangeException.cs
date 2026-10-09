namespace ECommerce.Orders.Exceptions;

using Griffin.Core.Exception;

public class InvalidTotalPriceRangeException : BadRequestException
{
    public InvalidTotalPriceRangeException(decimal totalPrice)
        : base($"TotalPrice: '{totalPrice}' must be grater than 50000.")
    {
    }
}
