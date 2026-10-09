namespace Unit.Test.Inventories.Features;

using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Exceptions;
using ECommerce.Inventories.Features.SellingProduct;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

public class SellProductTests : IDisposable
{
    private readonly UnitTestFixture _fixture;
    private readonly SellProductHandler _handler;

    public SellProductTests()
    {
        _fixture = new UnitTestFixture();
        _handler = new SellProductHandler(_fixture.DbContext);
    }

    public void Dispose() => _fixture.Dispose();

    private async Task Act(SellProduct command, CancellationToken cancellationToken)
    {
        await _handler.Handle(command, cancellationToken);
        await _fixture.DbContext.SaveChangesAsync(cancellationToken);
    }

    [Fact]
    public async Task handler_with_valid_command_should_decrease_quantity_of_in_stock_item()
    {
        // Arrange
        var command = new FakeSellProductCommand().Generate();

        // Act
        await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.InventoryItems
            .SingleOrDefaultAsync(x => x.ProductId == command.ProductId && x.Status == ProductStatus.InStock);

        entity.Should().NotBeNull();
        entity?.Quantity.Should().Be(0);
    }

    [Fact]
    public async Task handler_with_product_not_in_inventory_should_throw_product_not_exist_to_inventory_exception()
    {
        // Arrange
        var command = new FakeSellProductCommand().Generate() with { ProductId = Guid.NewGuid() };

        // Act
        var act = async () => await Act(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ProductNotExistToInventoryException>();
    }

    [Fact]
    public async Task handler_with_quantity_greater_than_stock_should_throw_out_of_range_quantity_exception()
    {
        // Arrange
        var command = new FakeSellProductCommand().Generate() with { Quantity = 1000 };

        // Act
        var act = async () => await Act(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<OutOfRangeQuantityException>();
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_null_exception()
    {
        // Arrange
        SellProduct command = null;

        // Act
        var act = async () => await Act(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
