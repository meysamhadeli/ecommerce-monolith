namespace ECommerce.Categories.Exceptions;

using Griffin.Core.Exception;

public class InvalidCategoryIdExceptions : BadRequestException
{
    public InvalidCategoryIdExceptions(Guid categoryId)
        : base($"CategoryId: '{categoryId}' is invalid.")
    {
    }
}