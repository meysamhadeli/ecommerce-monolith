namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Inventories.Features.ChangingProductStatus;

public class FakeValidateChangeProductStatus : AutoFaker<ChangeProductStatus>
{
    public FakeValidateChangeProductStatus()
    {
        RuleFor(r => r.ProductId, _ => Guid.Empty);
    }
}


