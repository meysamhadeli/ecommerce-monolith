namespace Integration.Test.Inventories.Features;

using BuildingBlocks.TestBase;
using ECommerce.Data;
using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Models;
using ECommerce.Products.ValueObjects;
using Fakes;
using FluentAssertions;
using Xunit;

public class ChangingProductStatusTests : ECommerceIntegrationTestBase
{
    public ChangingProductStatusTests(
        TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_change_product_status_to_inventory_db()
    {
        //Arrange
        var command = new FakeChangeProductStatusCommand().Generate();

        // Act
        await Fixture.SendAsync(command);

        // Assert
        var result = (await Fixture.GetAllAsync<InventoryItems>())?
            .Where(x=>x.ProductId == ProductId.Of(command.ProductId));

        result.Should().NotBeNull();
        result.Select(x=> x.Status.Should().Be(ProductStatus.Damaged));
    }
}
