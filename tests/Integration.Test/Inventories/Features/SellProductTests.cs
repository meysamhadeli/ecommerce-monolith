namespace Integration.Test.Inventories.Features;

using ECommerce.TestBase;
using ECommerce.Data;
using ECommerce.Inventories.Enums;
using Fakes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class SellProductTests : ECommerceIntegrationTestBase
{
    public SellProductTests(
        TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_set_sell_product_to_inventory_db()
    {
        //Arrange
        var command = new FakeSellProductCommand().Generate();

        // Act
        await Fixture.SendAsync(command);

        // Assert
        var result = await Fixture.ExecuteDbContextAsync(db =>
            db.InventoryItems
                .Where(x => x.ProductId == command.ProductId && x.Status == ProductStatus.Sold)
                .ToListAsync());

        result.Should().NotBeEmpty();
        result.Should().OnlyContain(x => x.Quantity >= 4);
    }
}
