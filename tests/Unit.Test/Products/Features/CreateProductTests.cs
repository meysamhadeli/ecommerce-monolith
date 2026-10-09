namespace Unit.Test.Products.Features;

using ECommerce.Products.Features.CreatingProduct;
using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

public class CreateProductTests : IDisposable
{
    private readonly UnitTestFixture _fixture;
    private readonly CreateProductHandler _handler;

    public CreateProductTests()
    {
        _fixture = new UnitTestFixture();
        _handler = new CreateProductHandler(_fixture.DbContext);
    }

    public void Dispose() => _fixture.Dispose();

    private async Task<CreateProductResult> Act(CreateProduct command, CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(command, cancellationToken);
        await _fixture.DbContext.SaveChangesAsync(cancellationToken);
        return result;
    }

    [Fact]
    public async Task handler_with_valid_command_should_create_new_product_and_return_currect_product_dto()
    {
        // Arrange
        var command = new FakeCreateProductCommand().Generate();

        // Act
        var response = await Act(command, CancellationToken.None);

        // Assert
        var entity = await _fixture.DbContext.Products.FindAsync(response.Id);

        entity.Should().NotBeNull();
        entity?.Id.Should().Be(response.Id);
        entity?.Name.Should().Be(command.Name);
        entity?.CategoryId.Should().Be(command.CategoryId);
        entity?.Barcode.Should().Be(command.Barcode);
    }

    [Fact]
    public async Task handler_with_null_command_should_throw_argument_null_exception()
    {
        // Arrange
        CreateProduct command = null;

        // Act
        var act = async () => await Act(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
