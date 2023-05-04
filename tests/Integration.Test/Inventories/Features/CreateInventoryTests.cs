namespace Integration.Test.Inventories.Features;

using BuildingBlocks.TestBase;
using ECommerce.Data;
using ECommerce.Inventories.Models;
using ECommerce.Inventories.ValueObjects;
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
        var result = await Fixture.FindAsync<Inventory, InventoryId>(inventoryEntity.Id);

        result.Should().NotBeNull();
        result?.Id.Value.Should().Be(inventoryEntity.Id.Value);
    }
}

