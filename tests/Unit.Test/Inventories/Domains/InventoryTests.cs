namespace Unit.Test.Inventories.Domains;

using ECommerce.Inventories.Models;
using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class InventoryTests
{
    [Fact]
    public void can_create_valid_inventory()
    {
        // Arrange + Act
        var fakeInventory = FakeInventory.Generate();

        // Assert
        fakeInventory.Should().NotBeNull();
        fakeInventory.DomainEvents.Count.Should().Be(1);
        fakeInventory.DomainEvents[0].Should().BeOfType(typeof(InventoryCreatedDomainEvent));
    }
}
