namespace ECommerce.Products.Exceptions;

using BuildingBlocks.Exception;

public class InvalidateNullOrEmptyDescriptionException : BadRequestException
{
    public InvalidateNullOrEmptyDescriptionException(string description)
        : base($"Description: '{description}' can not be null or empty.")
    {
    }
}
