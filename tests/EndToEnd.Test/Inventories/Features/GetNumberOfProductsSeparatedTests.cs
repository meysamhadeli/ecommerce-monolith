namespace EndToEnd.Test.Inventories.Features;

using System.Net;
using BuildingBlocks.TestBase;
using ECommerce.Data;
using FluentAssertions;
using Routes;
using Xunit;

public class GetNumberOfProductsSeparatedTests : ECommerceEndToEndTestBase
{
    public GetNumberOfProductsSeparatedTests(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }


    [Fact]
    public async Task should_retrive_nember_of_product_seprated_by_status_inventory_and_return_200()
    {
        //Arrange
        var route = ApiRoutes.Inventory.GetNumberOfProductsSeparated;

        // Act
        var result = await Fixture.HttpClient.GetAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}


