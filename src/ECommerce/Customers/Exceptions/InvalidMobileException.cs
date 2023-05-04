namespace ECommerce.Customers.Exceptions;

using BuildingBlocks.Exception;

public class InvalidMobileException : BadRequestException
{
    public InvalidMobileException(string mobile)
        : base($"Mobile: '{mobile}' is invalid.")
    {
    }
}
