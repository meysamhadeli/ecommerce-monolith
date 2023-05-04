namespace Unit.Test.Inventories.Domains;

using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Features.AddingProductToInventory;
using ECommerce.Inventories.Features.ChangingProductStatus;
using ECommerce.Inventories.Features.DamagingProduct;
using ECommerce.Inventories.Features.SellingProduct;
using FluentAssertions;
using Unit.Test.Common;
using Unit.Test.Fakes;
using Xunit;

[Collection(nameof(UnitTestFixture))]
public class InventoryItemsTests
{
    [Fact]
    public void can_add_product_to_inventory()
    {
        // Arrange + Act
        var fakeInventory = FakeAddProductToInventory.Generate();

        // Assert
        fakeInventory.Should().NotBeNull();
        fakeInventory.DomainEvents.Count.Should().Be(1);
        fakeInventory.DomainEvents[0].Should().BeOfType(typeof(ProductAddedToInventoryDomainEvent));
    }

    [Fact]
    public void can_change_product_status_to_inventory()
    {
        // Arrange + Act
        var fakeInventory = FakeChangeProductStatus.Generate(ProductStatus.Sold, ProductStatus.Damaged);

        // Assert
        fakeInventory.Should().NotBeNull();
        fakeInventory.Status.Should().Be(ProductStatus.Damaged);
        fakeInventory.DomainEvents.Count.Should().Be(2);
        fakeInventory.DomainEvents.Select(x=> x.Should().BeOfType(typeof(ProductAddedToInventoryDomainEvent)));
        fakeInventory.DomainEvents.Select(x=> x.Should().BeOfType(typeof(ProductStatusChangedDomainEvent)));
    }

    [Fact]
    public void can_sell_product_to_inventory()
    {
        // Arrange + Act
        var fakeInventory = FakeSellProduct.Generate(10, 2);

        // Assert
        fakeInventory.Should().NotBeNull();
        fakeInventory.Quantity.Value.Should().Be(8);
        fakeInventory.DomainEvents.Count.Should().Be(2);
        fakeInventory.DomainEvents.Select(x=> x.Should().BeOfType(typeof(ProductAddedToInventoryDomainEvent)));
        fakeInventory.DomainEvents.Select(x=> x.Should().BeOfType(typeof(ProductSoldDomainEvent)));
    }

    [Fact]
    public void can_damage_product_to_inventory()
    {
        // Arrange + Act
        var fakeInventory = FakeDamageProduct.Generate(6, 3);

        // Assert
        fakeInventory.Should().NotBeNull();
        fakeInventory.Quantity.Value.Should().Be(3);
        fakeInventory.DomainEvents.Count.Should().Be(2);
        fakeInventory.DomainEvents.Select(x=> x.Should().BeOfType(typeof(ProductAddedToInventoryDomainEvent)));
        fakeInventory.DomainEvents.Select(x=> x.Should().BeOfType(typeof(ProductDamagedDomainEvent)));
    }

    [Fact]
    public void can_update_product_to_inventory()
    {
        // Arrange + Act
        var fakeInventory = FakeUpdateProductToInventory.Generate(10, ProductStatus.Damaged);

        // Assert
        fakeInventory.Should().NotBeNull();
        fakeInventory.Quantity.Value.Should().Be(10);
        fakeInventory.Status.Should().Be(ProductStatus.Damaged);
        fakeInventory.DomainEvents.Count.Should().Be(2);
        fakeInventory.DomainEvents.Select(x=> x.Should().BeOfType(typeof(ProductAddedToInventoryDomainEvent)));
        fakeInventory.DomainEvents.Select(x=> x.Should().BeOfType(typeof(ProductUpdatedToInventoryDomainEvent)));
    }
}
