namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Orders.Dtos;
using ECommerce.Orders.Features.RegisteringNewOrder;

public class FakeValidateRegisterNewOrder : AutoFaker<RegisterNewOrder>
{
    public FakeValidateRegisterNewOrder()
    {
        RuleFor(r => r.Items, _ => new List<ItemDto>());
        RuleFor(r => r.DiscountValue, _ => -1);
        RuleFor(r => r.CustomerId, _ => Guid.Empty);
    }
}
