namespace Unit.Test.Inventories.Features;

using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Features.DamagingProduct;
using ECommerce.Products.ValueObjects;
using FluentAssertions;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class DamageProductTests
{
    private readonly UnitTestFixture _fixture;
    private readonly DamageProductHandler _handler;

    public Task<Unit> Act(DamageProduct command, CancellationToken cancellationToken) =>
        _handler.Handle(command, cancellationToken);

    public DamageProductTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new DamageProductHandler(fixture.DbContext);
    }

    [Fact]
    public void is_valid_should_be_false_when_validation_parameters_is_invalid()
    {
        // Arrange
        var command = new FakeValidateDamageProduct().Generate();
        var validator = new DamageProductValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public async Task handler_with_valid_command_should_damage_product_and_set_record_quantity_of_damaged()
    {
        // Arrange
        var command = new FakeDamageProductCommand().Generate();

        // Act
        await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.InventoryItems.SingleOrDefaultAsync(x =>
            x.ProductId == ProductId.Of(command.ProductId) && x.Status == ProductStatus.InStock);

        entity?.Should().NotBeNull();
        entity?.Quantity.Value.Should().Be(0);
        entity?.ProductId.Value.Should().Be(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c2"));
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        DamageProduct command = null;

        // Act
        Func<Task> act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
