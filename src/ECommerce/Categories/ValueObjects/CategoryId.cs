namespace ECommerce.Categories.ValueObjects;

using Exceptions;

public record CategoryId
{
    public Guid Value { get; }

    private CategoryId(Guid value)
    {
        Value = value;
    }

    public static CategoryId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidCategoryIdExceptions(value);
        }

        return new CategoryId(value);
    }

    public static implicit operator Guid(CategoryId categoryId) => categoryId.Value;
}
