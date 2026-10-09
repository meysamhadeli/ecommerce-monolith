namespace ECommerce.Orders.Exceptions;

using Griffin.Core.Exception;

public class InvalidDateTimeRangeException: BadRequestException
{
    public InvalidDateTimeRangeException(int? code = null) : base("You can only order between 8:00 AM until 7:00 PM", code)
    {
    }
}
