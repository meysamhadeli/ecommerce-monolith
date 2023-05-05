namespace Integration.Test.Orders.Features;

using BuildingBlocks.TestBase;
using ECommerce.Data;
using ECommerce.Orders.Enums;
using Fakes;
using FluentAssertions;
using Xunit;

public class RegisterNewOrderTests : ECommerceIntegrationTestBase
{
    public RegisterNewOrderTests(
        TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_register_new_order_to_db_and_return_order_result()
    {
        //Arrange
        var command = new FakeRegisterNewOrderCommand().Generate();

        // Act
        var response = await Fixture.SendAsync(command);

        // Assert
        response?.Should().NotBeNull();
        response?.Status.Should().Be(OrderStatus.Pending.ToString());
        response?.DiscountValue.Should().Be(command.DiscountValue);
        response?.DiscountType.Should().Be(command.DiscountType.ToString());
        response?.OrderDate.Should().Be(command.OrderDate);
        response?.ExpressOrderItems.Count().Should().Be(2);
    }
}

