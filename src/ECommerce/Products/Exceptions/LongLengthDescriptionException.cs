namespace ECommerce.Products.Exceptions;

using BuildingBlocks.Exception;

public class LongLengthDescriptionException : BadRequestException
{
    public LongLengthDescriptionException(string description, int maxLength)
        : base($"Description: '{description}' cannot be longer than {maxLength} characters")
    {
    }
}