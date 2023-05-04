namespace Unit.Test.Categories.Domains;

using ECommerce.Categories.Features.CreatingCategory;
using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class CategoryTests
{
    [Fact]
    public void can_create_valid_category()
    {
        // Arrange + Act
        var fakeCategory = FakeCategory.Generate();

        // Assert
        fakeCategory.Should().NotBeNull();
        fakeCategory.DomainEvents.Count.Should().Be(1);
        fakeCategory.DomainEvents[0].Should().BeOfType(typeof(CategoryCreatedDomainEvent));
    }
}
