namespace Unit.Test.Products.Features;

using ECommerce.Products.Features.CreatingProduct;
using ECommerce.Products.ValueObjects;
using FluentAssertions;
using FluentValidation.TestHelper;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class CreateProductTests
{
    private readonly UnitTestFixture _fixture;
    private readonly CreateProductHandler _handler;

    public Task<CreateProductResult> Act(CreateProduct command, CancellationToken cancellationToken) =>
        _handler.Handle(command, cancellationToken);

    public CreateProductTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new CreateProductHandler(fixture.DbContext);
    }

    [Fact]
    public void is_valid_should_be_false_when_validation_parameters_is_invalid()
    {
        // Arrange
        var command = new FakeValidateCreateProduct().Generate();
        var validator = new CreateProductValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Barcode);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
        result.ShouldHaveValidationErrorFor(x => x.ProfitMargin);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public async Task handler_with_valid_command_should_create_product_and_return_valid_product_id()
    {
        // Arrange
        var command = new FakeCreateProductCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.Products.FindAsync(ProductId.Of(response.Id));

        entity?.Should().NotBeNull();
        entity?.Id.Value.Should().Be(response.Id);
        entity?.CategoryId.Value.Should().Be(command.CategoryId);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        CreateProduct command = null;

        // Act
        Func<Task> act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}

