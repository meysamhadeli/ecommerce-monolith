namespace ECommerce.Inventories.Exceptions;

using Griffin.Core.Exception;

public class InvalidStatusInStockException : BadRequestException
{
    public InvalidStatusInStockException(int? code = default) : base("Invalid Status, Status must be InStock!", code)
    {
    }
}

