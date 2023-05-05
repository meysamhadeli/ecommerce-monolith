namespace EndToEnd.Test.Products.Features;

using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.TestBase;
using ECommerce.Data;
using Fakes;
using Routes;
using FluentAssertions;
using Xunit;

public class CreateProductTests : ECommerceEndToEndTestBase
{
    public CreateProductTests(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }


    [Fact]
    public async Task should_create_new_product_and_return_200()
    {
        //Arrange
        var command = new FakeCreateProductCommand().Generate();

        // Act
        var route = ApiRoutes.Catalog.CreateProduct;
        var result = await Fixture.HttpClient.PostAsJsonAsync(route, command);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

