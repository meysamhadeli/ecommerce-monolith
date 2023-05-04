namespace EndToEnd.Test.Fakes;

using AutoBogus;
using ECommerce.Categories.Features.CreatingCategory;
using MassTransit;

public sealed class FakeCreateCategoryCommand : AutoFaker<CreateCategory>
{
    public FakeCreateCategoryCommand()
    {
        RuleFor(r => r.Id, _ => NewId.NextGuid());
        RuleFor(r => r.Name, r => "Food");
    }
}
