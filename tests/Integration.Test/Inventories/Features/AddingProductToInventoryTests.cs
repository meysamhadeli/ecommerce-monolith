namespace Integration.Test.Inventories.Features;

using BuildingBlocks.TestBase;
using ECommerce.Data;
using ECommerce.Inventories.ValueObjects;
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
            db.InventoryItems.FindAsync(InventoryItemsId.Of(response.Id)));

        result.Should().NotBeNull();
        result?.Quantity.Value.Should().BeGreaterOrEqualTo(5);
    }
}
