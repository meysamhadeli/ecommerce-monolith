namespace ECommerce.Products.Exceptions;

using Griffin.Core.Exception;

public class ShortLengthDescriptionException : BadRequestException
{
    public ShortLengthDescriptionException(string description, int minLength)
        : base($"Description: '{description}' cannot be less than {minLength} characters")
    {
    }
}
