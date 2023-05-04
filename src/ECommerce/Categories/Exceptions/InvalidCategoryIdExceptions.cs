namespace ECommerce.Categories.Exceptions;

using BuildingBlocks.Exception;

public class InvalidCategoryIdExceptions : BadRequestException
{
    public InvalidCategoryIdExceptions(Guid categoryId)
        : base($"CategoryId: '{categoryId}' is invalid.")
    {
    }
}