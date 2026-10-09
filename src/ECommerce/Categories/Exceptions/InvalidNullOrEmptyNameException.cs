namespace ECommerce.Categories.Exceptions;

using Griffin.Core.Exception;

public class InvalidNullOrEmptyNameException : BadRequestException
{
    public InvalidNullOrEmptyNameException(string name)
        : base($"Name: '{name}' can not be null or empty.")
    {
    }
}
