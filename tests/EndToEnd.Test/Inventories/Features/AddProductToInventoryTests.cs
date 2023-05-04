namespace EndToEnd.Test.Inventories.Features;

using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.TestBase;
using ECommerce.Data;
using Fakes;
using FluentAssertions;
using Routes;
using Xunit;

public class AddProductToInventoryTests : ECommerceEndToEndTestBase
{
    public AddProductToInventoryTests(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }


    [Fact]
    public async Task should_add_new_product_to_inventory_and_return_200()
    {
        //Arrange
        var command = new FakeAddProductToInventoryCommand().Generate();

        // Act
        var route = ApiRoutes.Inventory.AddProductToInventory;
        var result = await Fixture.HttpClient.PostAsJsonAsync(route, command);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}


