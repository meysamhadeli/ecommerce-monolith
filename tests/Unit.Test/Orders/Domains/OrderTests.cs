namespace Unit.Test.Orders.Domains;

using Common;
using ECommerce.Orders.Enums;
using ECommerce.Orders.Features.RegisteringNewOrder;
using Fakes;
using FluentAssertions;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class OrderTests
{
    [Fact]
    public void can_init_order_and_raise_domain_events_for_order_initialed_and_order_items_added_to_order()
    {
        // Arrange + Act
        var fakeOrder = FakeOrder.Generate();

        // Assert
        fakeOrder.Should().NotBeNull();
        fakeOrder.DomainEvents.Select(x=> x.Should().BeOfType(typeof(OrderInitialedDomainEvent)));
        fakeOrder.DomainEvents.Select(x=> x.Should().BeOfType(typeof(OrderItemsAddedToOrderDomainEvent)));
    }

    [Fact]
    public void can_apply_discount_and_raise_domain_event_order_discount_applied()
    {
        // Arrange
        var fakeOrder = FakeOrder.Generate();

        // Act
        fakeOrder.ApplyDiscount(DiscountType.PercentageDiscount, 30);

        // Assert
        fakeOrder.Should().NotBeNull();
        fakeOrder.DomainEvents.Select(x=> x.Should().BeOfType(typeof(OrderDiscountAppliedDomainEvent)));
    }

    [Fact]
    public void can_apply_calculate_total_price_and_raise_domain_event_order_total_price_added()
    {
        // Arrange
        var fakeOrder = FakeOrder.Generate();

        // Act
        fakeOrder.CalculateTotalPrice();

        // Assert
        fakeOrder.Should().NotBeNull();
        fakeOrder.DomainEvents.Select(x=> x.Should().BeOfType(typeof(OrderTotalPriceAddedDomainEvent)));
    }


    [Fact]
    public void can_apply_shipment_order_and_and_raise_order_shipment_applied()
    {
        // Arrange
        var fakeOrder = FakeOrder.Generate();

        // Act
        var (expressShipmentItems, _) = fakeOrder.ApplyShipment();

        // Assert
        fakeOrder.Should().NotBeNull();
        fakeOrder.DomainEvents.Select(x=> x.Should().BeOfType(typeof(OrderShipmentAppliedDomainEvent)));
        expressShipmentItems.Count().Should().Be(1);
    }
}
