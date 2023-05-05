namespace EndToEnd.Test.Orders.Features;

using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.TestBase;
using ECommerce.Data;
using Fakes;
using FluentAssertions;
using Routes;
using Xunit;

public class RegisterNewOrderTests : ECommerceEndToEndTestBase
{
    public RegisterNewOrderTests(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }


    [Fact]
    public async Task should_register_new_order_and_return_200()
    {
        //Arrange
        var command = new FakeRegisterNewOrderCommand().Generate();

        // Act
        var route = ApiRoutes.Order.RegisterNewOrder;
        var result = await Fixture.HttpClient.PostAsJsonAsync(route, command);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

