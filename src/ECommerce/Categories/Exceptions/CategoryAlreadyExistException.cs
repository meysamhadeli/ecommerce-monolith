namespace ECommerce.Categories.Exceptions;

using BuildingBlocks.Exception;

public class CategoryAlreadyExistException : ConflictException
{
    public CategoryAlreadyExistException(int? code = default) : base("Category already exist!", code)
    {
    }
}
