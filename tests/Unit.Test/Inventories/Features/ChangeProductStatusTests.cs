namespace Unit.Test.Inventories.Features;

using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Features.ChangingProductStatus;
using ECommerce.Products.ValueObjects;
using FluentAssertions;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class ChangeProductStatusTests
{
    private readonly UnitTestFixture _fixture;
    private readonly ChangeProductStatusHandler _handler;

    public Task<Unit> Act(ChangeProductStatus command, CancellationToken cancellationToken) =>
        _handler.Handle(command, cancellationToken);

    public ChangeProductStatusTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new ChangeProductStatusHandler(fixture.DbContext);
    }

    [Fact]
    public void is_valid_should_be_false_when_validation_parameters_is_invalid()
    {
        // Arrange
        var command = new FakeValidateChangeProductStatus().Generate();
        var validator = new ChangeProductStatusValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Fact]
    public async Task handler_with_valid_command_should_change_product_status_and_change_status_with_new_value()
    {
        // Arrange
        var command = new FakeChangeProductStatusCommand().Generate();

        // Act
        await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.InventoryItems.Where(x =>
            x.ProductId == ProductId.Of(command.ProductId)).ToListAsync();

        entity?.Should().NotBeNull();
        entity?.Select(x=> x.ProductId.Value.Should().Be(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3")));
        entity?.Select(x=> x.Status.Should().Be(ProductStatus.Damaged));
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        ChangeProductStatus command = null;

        // Act
        Func<Task> act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}

