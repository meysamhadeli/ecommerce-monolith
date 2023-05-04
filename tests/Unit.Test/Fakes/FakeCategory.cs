namespace Unit.Test.Fakes;

using ECommerce.Categories.Models;
using ECommerce.Categories.ValueObjects;
using MassTransit;

public static class FakeCategory
{
    public static Category Generate()
    {
        return Category.Create(CategoryId.Of(NewId.NextGuid()), Name.Of("Food") );
    }
}
