namespace ECommerce.Customers.Exceptions;

using Griffin.Core.Exception;

public class InvalidMobileException : BadRequestException
{
    public InvalidMobileException(string mobile)
        : base($"Mobile: '{mobile}' is invalid.")
    {
    }
}
