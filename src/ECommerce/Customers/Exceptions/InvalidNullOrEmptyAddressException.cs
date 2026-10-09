namespace ECommerce.Customers.Exceptions;

using Griffin.Core.Exception;

public class InvalidNullOrEmptyAddressException : BadRequestException
{
    public InvalidNullOrEmptyAddressException()
        : base("Address can not be null or empty!")
    {
    }
}

