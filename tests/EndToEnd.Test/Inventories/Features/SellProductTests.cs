namespace EndToEnd.Test.Inventories.Features;

using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.TestBase;
using ECommerce.Data;
using Fakes;
using FluentAssertions;
using Routes;
using Xunit;

public class SellProductTests : ECommerceEndToEndTestBase
{
    public SellProductTests(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }


    [Fact]
    public async Task should_set_sell_product_to_inventory_and_return_204()
    {
        //Arrange
        var command = new FakeSellProductCommand().Generate();

        // Act
        var route = ApiRoutes.Inventory.SellProduct;
        var result = await Fixture.HttpClient.PostAsJsonAsync(route, command);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}

