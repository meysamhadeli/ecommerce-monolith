namespace Integration.Test.Categories.Features;

using BuildingBlocks.TestBase;
using ECommerce.Data;
using Fakes;
using FluentAssertions;
using Xunit;


public class CreateCategoryTests : ECommerceIntegrationTestBase
{
    public CreateCategoryTests(
        TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_create_new_category_to_db()
    {
        //Arrange
        var command = new FakeCreateCategoryCommand().Generate();

        // Act
        var response = await Fixture.SendAsync(command);

        // Assert
        response.Should().NotBeNull();
        response?.Id.Should().Be(command.Id);
    }
}
