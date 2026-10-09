namespace Unit.Test.Inventories.Features;

using ECommerce.Inventories.Features.AddingProductToInventory;
using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

public class AddProductToInventoryTests : IDisposable
{
    private readonly UnitTestFixture _fixture;
    private readonly AddProductToInventoryHandler _handler;

    public AddProductToInventoryTests()
    {
        _fixture = new UnitTestFixture();
        _handler = new AddProductToInventoryHandler(_fixture.DbContext);
    }

    public void Dispose() => _fixture.Dispose();

    private async Task<AddProductToInventoryResult> Act(AddProductToInventory command, CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(command, cancellationToken);
        await _fixture.DbContext.SaveChangesAsync(cancellationToken);
        return result;
    }

    [Fact]
    public async Task handler_with_valid_command_should_add_product_to_inventory()
    {
        // Arrange
        var command = new FakeAddProductToInventoryCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.InventoryItems.FindAsync(response.Id);

        entity.Should().NotBeNull();
        entity?.Id.Should().Be(response.Id);
        entity?.Quantity.Should().Be(command.Quantity);
        entity?.ProductId.Should().Be(command.ProductId);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_null_exception()
    {
        // Arrange
        AddProductToInventory command = null;

        // Act
        var act = async () => await Act(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
