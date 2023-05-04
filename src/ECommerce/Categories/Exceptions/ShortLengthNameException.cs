namespace ECommerce.Categories.Exceptions;

using BuildingBlocks.Exception;

public class ShortLengthNameException : BadRequestException
{
    public ShortLengthNameException(string name, int minLength)
        : base($"Name: '{name}' cannot be less than {minLength} characters")
    {
    }
}
