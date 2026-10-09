namespace Integration.Test.Inventories.Features;

using ECommerce.TestBase;
using ECommerce.Data;
using Fakes;
using FluentAssertions;
using Xunit;

public class AddingProductToInventoryTests : ECommerceIntegrationTestBase
{
    public AddingProductToInventoryTests(
        TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_add_new_product_to_inventory_db()
    {
        //Arrange
        var command = new FakeAddProductToInventoryCommand().Generate();

        // Act
        var response = await Fixture.SendAsync(command);

        // Assert
        var result = await Fixture.ExecuteDbContextAsync(db =>
            db.InventoryItems.FindAsync(response.Id).AsTask());

        result.Should().NotBeNull();
        result?.Quantity.Should().BeGreaterThanOrEqualTo(5);
    }
}
