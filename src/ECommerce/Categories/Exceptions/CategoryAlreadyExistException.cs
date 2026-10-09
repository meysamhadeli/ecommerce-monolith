namespace ECommerce.Categories.Exceptions;

using Griffin.Core.Exception;

public class CategoryAlreadyExistException : ConflictException
{
    public CategoryAlreadyExistException(int? code = default) : base("Category already exist!", code)
    {
    }
}
