namespace EndToEnd.Test.Categories.Features;

using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.TestBase;
using ECommerce.Data;
using EndToEnd.Test.Fakes;
using EndToEnd.Test.Routes;
using FluentAssertions;
using Xunit;

public class CreateCategoryTests : ECommerceEndToEndTestBase
{
    public CreateCategoryTests(TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }


    [Fact]
    public async Task should_create_new_category_and_return_200()
    {
        //Arrange
        var command = new FakeCreateCategoryCommand().Generate();

        // Act
        var route = ApiRoutes.Catalog.CreateCategory;
        var result = await Fixture.HttpClient.PostAsJsonAsync(route, command);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

