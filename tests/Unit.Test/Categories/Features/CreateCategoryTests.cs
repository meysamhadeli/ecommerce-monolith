namespace Unit.Test.Categories.Features;

using ECommerce.Categories.Features.CreatingCategory;
using ECommerce.Categories.ValueObjects;
using FluentAssertions;
using FluentValidation.TestHelper;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class CreateCategoryTests
{
    private readonly UnitTestFixture _fixture;
    private readonly CreateCategoryHandler _handler;

    public Task<CreateCategoryResult> Act(CreateCategory command, CancellationToken cancellationToken) =>
        _handler.Handle(command, cancellationToken);

    public CreateCategoryTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new CreateCategoryHandler(fixture.DbContext);
    }

    [Fact]
    public void is_valid_should_be_false_when_validation_parameters_is_invalid()
    {
        // Arrange
        var command = new FakeValidateCreateCategory().Generate();
        var validator = new CreateCategoryValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task handler_with_valid_command_should_create_new_category_and_return_currect_create_category_result()
    {
        // Arrange
        var command = new FakeCreateCategoryCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.Categories.FindAsync(CategoryId.Of(response.Id));

        entity?.Should().NotBeNull();
        response?.Id.Should().Be(entity.Id.Value);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_exception()
    {
        // Arrange
        CreateCategory command = null;

        // Act
        Func<Task> act = async () => { await Act(command, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}

