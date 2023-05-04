namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Features.ChangingProductStatus;

public sealed class FakeChangeProductStatusCommand : AutoFaker<ChangeProductStatus>
{
    public FakeChangeProductStatusCommand()
    {
        RuleFor(r => r.ProductId, r => new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3"));
        RuleFor(r => r.OldStatus, _ => ProductStatus.InStock);
        RuleFor(r => r.NewStatus, _ => ProductStatus.Damaged);
    }
}

