namespace ECommerce.Customers.Exceptions;

using BuildingBlocks.Exception;

public class InvalidNullOrEmptyAddressException : BadRequestException
{
    public InvalidNullOrEmptyAddressException()
        : base("Address can not be null or empty!")
    {
    }
}

