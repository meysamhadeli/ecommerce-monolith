namespace Unit.Test.Inventories.Features;

using ECommerce.Inventories.Features.AddingProductToInventory;
using ECommerce.Inventories.ValueObjects;
using FluentAssertions;
using FluentValidation.TestHelper;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class AddProductToInventoryTests
{
    private readonly UnitTestFixture _fixture;
    private readonly AddProductToInventoryHandler _handler;

    public Task<AddProductToInventoryResult> Act(AddProductToInventory command, CancellationToken cancellationToken) =>
        _handler.Handle(command, cancellationToken);

    public AddProductToInventoryTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new AddProductToInventoryHandler(fixture.DbContext);
    }

    [Fact]
    public void is_valid_should_be_false_when_validation_parameters_is_invalid()
    {
        // Arrange
        var command = new FakeValidateAddProductToInventory().Generate();
        var validator = new AddProductToInventoryValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
        result.ShouldHaveValidationErrorFor(x => x.InventoryId);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public async Task handler_with_valid_command_should_add_product_to_inventory_and_return_add_product_to_inventory_result()
    {
        // Arrange
        var command = new FakeAddProductToInventoryCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.InventoryItems.FindAsync(InventoryItemsId.Of(response.Id));

        entity?.Should().NotBeNull();
        response?.Id.Should().Be(entity.Id.Value);
        entity?.Quantity.Value.Should().Be(5);
        entity?.ProductId.Value.Should().Be(new Guid("1c5c0000-97c6-fc34-fcd3-08db322230c0"));
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        AddProductToInventory command = null;

        // Act
        Func<Task> act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}

