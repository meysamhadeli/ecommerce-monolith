namespace ECommerce.Inventories.Exceptions;

using Griffin.Core.Exception;

public class InvalidStatusSoldException : BadRequestException
{
    public InvalidStatusSoldException(int? code = default) : base("Invalid Status, Status must be Sold!", code)
    {
    }
}
