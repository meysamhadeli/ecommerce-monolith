namespace Unit.Test.Inventories.Features;

using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Features.GettingNumberOfProductsSeparated;
using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class GetNumberOfProductsSeparatedTests
{
    private readonly UnitTestFixture _fixture;
    private readonly GetNumberOfProductsSeparatedHandler _handler;

    public Task<IEnumerable<GetNumberOfProductsSeparatedResult>> Act(GetNumberOfProductsSeparated command,
        CancellationToken cancellationToken) =>
        _handler.Handle(command, cancellationToken);

    public GetNumberOfProductsSeparatedTests(UnitTestFixture fixture)
    {
        _fixture = fixture;
        _handler = new GetNumberOfProductsSeparatedHandler(fixture.DbContext);
    }

    [Fact]
    public async Task handler_with_valid_query_should_return_number_of_products_separated()
    {
        // Arrange
        var query = new FakeGetNumberOfProductsSeparatedQuery().Generate();

        // Act
        var response = await Act(query, CancellationToken.None);

        // Assert
        response?.Should().NotBeNull();
        response.Select(x=>x.Status.Should().Be(ProductStatus.Damaged.ToString()));
        response.Select(x=>x.Status.Should().Be(ProductStatus.Sold.ToString()));
        response.Select(x=>x.Status.Should().Be(ProductStatus.InStock.ToString()));
    }

    [Fact]
    public async Task handler_with_null_query_should_throw_argument_exception()
    {
        // Arrange
        GetNumberOfProductsSeparated query = null;

        // Act
        Func<Task> act = async () => { await Act(query, CancellationToken.None); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
