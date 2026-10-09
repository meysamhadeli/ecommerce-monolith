namespace Unit.Test.Categories.Features;

using ECommerce.Categories.Exceptions;
using ECommerce.Categories.Features.CreatingCategory;
using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

public class CreateCategoryTests : IDisposable
{
    private readonly UnitTestFixture _fixture;
    private readonly CreateCategoryHandler _handler;

    public CreateCategoryTests()
    {
        _fixture = new UnitTestFixture();
        _handler = new CreateCategoryHandler(_fixture.DbContext);
    }

    public void Dispose() => _fixture.Dispose();

    private async Task<CreateCategoryResult> Act(CreateCategory command, CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(command, cancellationToken);
        await _fixture.DbContext.SaveChangesAsync(cancellationToken);
        return result;
    }

    [Fact]
    public async Task handler_with_valid_command_should_create_new_category_and_return_currect_category_dto()
    {
        // Arrange
        var command = new FakeCreateCategoryCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.Categories.FindAsync(response.Id);

        entity.Should().NotBeNull();
        entity?.Id.Should().Be(response.Id);
        entity?.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_null_exception()
    {
        // Arrange
        CreateCategory command = null;

        // Act
        var act = async () => await Act(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task handler_with_existing_category_should_throw_category_already_exist_exception()
    {
        // Arrange
        var command = new FakeCreateCategoryCommand().Generate();
        await Act(command, CancellationToken.None);

        // Act
        var act = async () => await Act(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CategoryAlreadyExistException>();
    }
}
