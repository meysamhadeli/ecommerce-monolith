namespace Integration.Test.Inventories.Features;

using ECommerce.TestBase;
using ECommerce.Data;
using Fakes;
using FluentAssertions;
using Xunit;

public class CreateInventoryTests : ECommerceIntegrationTestBase
{
    public CreateInventoryTests(
        TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_create_new_inventory_to_db()
    {
        //Arrange
        var inventoryEntity = FakeInventory.Generate();

        // Act
        await Fixture.InsertAsync(inventoryEntity);

        // Assert
        var result = await Fixture.ExecuteDbContextAsync(db =>
            db.Inventories.FindAsync(inventoryEntity.Id).AsTask());

        result.Should().NotBeNull();
        result?.Id.Should().Be(inventoryEntity.Id);
    }
}
