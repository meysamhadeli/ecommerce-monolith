namespace ECommerce.Inventories.Models;

using BuildingBlocks.Core.Model;
using Enums;
using Exceptions;
using Features.AddingProductToInventory;
using Features.DamagingProduct;
using Features.SellingProduct;
using Products.Models;
using Products.ValueObjects;
using ValueObjects;

public record InventoryItems : Aggregate<InventoryItemsId>
{
    public InventoryId InventoryId { get; private set; }
    public Inventory Inventory { get; private set; }
    public ProductId ProductId { get; private set; }
    public Product Product { get; private set; }
    public Quantity Quantity { get; private set; }
    public ProductStatus Status { get; private set; }


    public static InventoryItems AddProductToInventory(InventoryItemsId id, InventoryId inventoryId,
        ProductId productId, Quantity quantity,
        ProductStatus status = ProductStatus.InStock, bool isDeleted = false)
    {
        var inventoryItems = new InventoryItems
        {
            Id = id,
            InventoryId = inventoryId,
            ProductId = productId,
            Quantity = quantity,
            Status = status,
            IsDeleted = isDeleted
        };

        var @event = new ProductAddedToInventoryDomainEvent(inventoryItems.Id.Value, inventoryItems.InventoryId.Value,
            inventoryItems.ProductId.Value,
            inventoryItems.Status, inventoryItems.Quantity.Value);

        inventoryItems.AddDomainEvent(@event);

        return inventoryItems;
    }

    public void SellProduct(InventoryItemsId id, InventoryId inventoryId, ProductId productId, Quantity quantity,
        ProductStatus status = ProductStatus.InStock, bool isDeleted = false)
    {
        var realNowQuantity = Quantity.Value - quantity.Value;

        if (realNowQuantity < 0)
        {
            throw new ProductQuantityException();
        }

        Id = id;
        ProductId = productId;
        Quantity = Quantity.Of(realNowQuantity);
        Status = status;
        InventoryId = inventoryId;
        IsDeleted = isDeleted;

        var @event = new ProductSoldDomainEvent(id.Value, inventoryId.Value, productId.Value, status, quantity.Value);

        AddDomainEvent(@event);
    }

    public void DamageProduct(InventoryItemsId id, InventoryId inventoryId, ProductId productId, Quantity quantity,
        ProductStatus status = ProductStatus.InStock, bool isDeleted = false)
    {
        var realNowQuantity = Quantity.Value - quantity.Value;

        if (realNowQuantity < 0)
        {
            throw new ProductQuantityException();
        }

        Id = id;
        ProductId = productId;
        Quantity = Quantity.Of(realNowQuantity);
        Status = status;
        InventoryId = inventoryId;
        IsDeleted = isDeleted;

        var @event = new ProductDamagedDomainEvent(id.Value, inventoryId.Value, productId.Value, status, quantity.Value);

        AddDomainEvent(@event);
    }

    public void UpdateProductToInventory(InventoryItemsId id, InventoryId inventoryId, ProductId productId,
        Quantity quantity,
        ProductStatus status = ProductStatus.InStock, bool isDeleted = false)
    {
        if (quantity.Value < 0)
        {
            throw new InvalidQuantityException(quantity.Value);
        }

        Id = id;
        ProductId = productId;
        Quantity = quantity;
        Status = status;
        InventoryId = inventoryId;
        IsDeleted = isDeleted;

        var @event = new ProductUpdatedToInventoryDomainEvent(id.Value, inventoryId.Value, productId.Value, status, quantity.Value);

        AddDomainEvent(@event);
    }
}
