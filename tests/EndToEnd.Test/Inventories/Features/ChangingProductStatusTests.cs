namespace EndToEnd.Test.Inventories.Features;

using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.TestBase;
using ECommerce.Data;
using Fakes;
using FluentAssertions;
using Routes;
using Xunit;

public class ChangingProductStatusTests : ECommerceEndToEndTestBase
{
    public ChangingProductStatusTests(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }


    [Fact]
    public async Task should_change_product_status_to_inventory_and_return_204()
    {
        //Arrange
        var command = new FakeChangeProductStatusCommand().Generate();

        // Act
        var route = ApiRoutes.Inventory.ChangeProductStatus;
        var result = await Fixture.HttpClient.PutAsJsonAsync(route, command);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
