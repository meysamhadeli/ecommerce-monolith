namespace EndToEnd.Test.Inventories.Features;

using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.TestBase;
using ECommerce.Data;
using Fakes;
using FluentAssertions;
using Routes;
using Xunit;

public class DamageProductTests : ECommerceEndToEndTestBase
{
    public DamageProductTests(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }


    [Fact]
    public async Task should_set_damage_product_to_inventory_and_return_204()
    {
        //Arrange
        var command = new FakeDamageProductCommand().Generate();

        // Act
        var route = ApiRoutes.Inventory.DamageProduct;
        var result = await Fixture.HttpClient.PostAsJsonAsync(route, command);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}

