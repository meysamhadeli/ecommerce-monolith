namespace Integration.Test.Inventories.Features;

using ECommerce.TestBase;
using ECommerce.Data;
using ECommerce.Inventories.Enums;
using Fakes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class DamageProductTests : ECommerceIntegrationTestBase
{
    public DamageProductTests(
        TestFixture<ECommerce.Api.Program, ECommerceDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async Task should_set_damage_product_to_inventory_db()
    {
        //Arrange
        var command = new FakeDamageProductCommand().Generate();

        // Act
        await Fixture.SendAsync(command);

        // Assert
        var result = await Fixture.ExecuteDbContextAsync(db =>
            db.InventoryItems
                .Where(x => x.ProductId == command.ProductId && x.Status == ProductStatus.Damaged)
                .ToListAsync());

        result.Should().NotBeEmpty();
        result.Should().OnlyContain(x => x.Quantity >= 5);
    }
}
