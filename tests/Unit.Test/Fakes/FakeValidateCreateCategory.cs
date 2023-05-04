namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Categories.Features.CreatingCategory;

public class FakeValidateCreateCategory : AutoFaker<CreateCategory>
{
    public FakeValidateCreateCategory()
    {
        RuleFor(r => r.Name, _ => string.Empty);
    }
}
