namespace Unit.Test.Inventories.Features;

using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Features.SellingProduct;
using ECommerce.Products.ValueObjects;
using FluentAssertions;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class SellProductTests
{
    private readonly UnitTestFixture _fixture;
    private readonly SellProductHandler _handler;

    public Task<Unit> Act(SellProduct command, CancellationToken cancellationToken) =>
        _handler.Handle(command, cancellationToken);

    public SellProductTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new SellProductHandler(fixture.DbContext);
    }

    [Fact]
    public void is_valid_should_be_false_when_validation_parameters_is_invalid()
    {
        // Arrange
        var command = new FakeValidateSellProduct().Generate();
        var validator = new SellProductValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public async Task handler_with_valid_command_should_sell_product_and_set_record_quantity_of_sold()
    {
        // Arrange
        var command = new FakeSellProductCommand().Generate();

        // Act
        await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.InventoryItems.SingleOrDefaultAsync(x =>
            x.ProductId == ProductId.Of(command.ProductId) && x.Status == ProductStatus.InStock);

        entity?.Should().NotBeNull();
        entity?.Quantity.Value.Should().Be(0);
        entity?.ProductId.Value.Should().Be(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3"));
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        SellProduct command = null;

        // Act
        Func<Task> act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
