namespace ECommerce.Categories.Models;

using BuildingBlocks.Core.Model;
using Features.CreatingCategory;
using ValueObjects;

public record Category : Aggregate<CategoryId>
{
    public Name Name { get; private set; }

    public static Category Create(CategoryId id, Name name, bool isDeleted = false)
    {
        var category = new Category
        {
            Id = id,
            Name = name,
            IsDeleted = isDeleted
        };

        var @event = new CategoryCreatedDomainEvent(category.Id.Value, category.Name.Value, category.IsDeleted);

        category.AddDomainEvent(@event);

        return category;
    }
}
