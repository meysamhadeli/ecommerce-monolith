namespace Unit.Test.Fakes;

using AutoBogus;
using ECommerce.Orders.Dtos;
using ECommerce.Orders.Enums;
using ECommerce.Orders.Features.RegisteringNewOrder;
using MassTransit;

public class FakeRegisterNewOrderCommand : AutoFaker<RegisterNewOrder>
{
    public FakeRegisterNewOrderCommand()
    {
        RuleFor(r => r.Id, _ => NewId.NextGuid());
        RuleFor(r => r.DiscountType, _ =>  DiscountType.AmountDiscount);
        RuleFor(r => r.DiscountValue, _ =>  1800);
        RuleFor(r => r.OrderDate, _ =>  new DateTime(2023, 1, 1, 18, 0, 0));
        RuleFor(r => r.CustomerId, _ => new Guid("2c5c0000-97c6-fc34-fcd3-08db322230c0"));
        RuleFor(r => r.Items, _ => new List<ItemDto>(new []
        {
            new ItemDto(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0"), 2),
            new ItemDto(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3"), 4)
        }));
    }
}

