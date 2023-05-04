namespace Integration.Test.Products.Features;

using BuildingBlocks.TestBase;
using ECommerce.Data;
using Fakes;
using FluentAssertions;
using Xunit;

public class CreateProductTests : ECommerceIntegrationTestBase
{
    public CreateProductTests(
        TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_create_new_product_to_db()
    {
        //Arrange
        var command = new FakeCreateProductCommand().Generate();

        // Act
        var response = await Fixture.SendAsync(command);

        // Assert
        response.Should().NotBeNull();
        response?.Id.Should().Be(command.Id);
    }
}

