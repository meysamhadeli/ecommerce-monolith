namespace Unit.Test.Orders.Features;

using ECommerce.Orders.Enums;
using ECommerce.Orders.Features.RegisteringNewOrder;
using FluentAssertions;
using FluentValidation.TestHelper;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

public class RegisterNewOrderTests : IDisposable
{
    private readonly UnitTestFixture _fixture;
    private readonly RegisterNewOrderHandler _handler;

    public RegisterNewOrderTests()
    {
        _fixture = new UnitTestFixture();
        _handler = new RegisterNewOrderHandler(_fixture.DbContext);
    }

    public void Dispose() => _fixture.Dispose();

    private async Task<RegisterNewOrderResult> Act(RegisterNewOrder command, CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(command, cancellationToken);
        await _fixture.DbContext.SaveChangesAsync(cancellationToken);
        return result;
    }

    [Fact]
    public void is_valid_should_be_false_when_validation_parameters_is_invalid()
    {
        // Arrange
        var command = new FakeValidateRegisterNewOrder().Generate();
        var validator = new RegisterNewOrderValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Items);
        result.ShouldHaveValidationErrorFor(x => x.DiscountValue);
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }

    [Fact]
    public async Task handler_with_valid_command_should_register_new_order_and_return_valid_order_result()
    {
        // Arrange
        var command = new FakeRegisterNewOrderCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        response?.Should().NotBeNull();
        response?.Status.Should().Be(OrderStatus.Pending.ToString());
        response?.DiscountValue.Should().Be(command.DiscountValue);
        response?.DiscountType.Should().Be(command.DiscountType.ToString());
        response?.OrderDate.Should().Be(command.OrderDate);
        response?.ExpressOrderItems.Count().Should().Be(2);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        RegisterNewOrder command = null;

        // Act
        Func<Task> act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
