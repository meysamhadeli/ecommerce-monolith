namespace ECommerce.Categories.Exceptions;

using BuildingBlocks.Exception;

public class InvalidNullOrEmptyNameException : BadRequestException
{
    public InvalidNullOrEmptyNameException(string name)
        : base($"Name: '{name}' can not be null or empty.")
    {
    }
}
