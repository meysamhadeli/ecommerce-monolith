namespace Unit.Test.Products.Domains;

using ECommerce.Products.Features.CreatingProduct;
using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class ProductTests
{
    [Fact]
    public void can_create_valid_product()
    {
        // Arrange + Act
        var fakeProduct = FakeProduct.Generate();

        // Assert
        fakeProduct.Should().NotBeNull();
        fakeProduct.DomainEvents.Count.Should().Be(1);
        fakeProduct.DomainEvents[0].Should().BeOfType(typeof(ProductCreatedDomainEvent));
    }
}
