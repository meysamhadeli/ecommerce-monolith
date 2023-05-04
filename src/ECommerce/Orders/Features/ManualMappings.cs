namespace ECommerce.Orders.Features;

using Dtos;
using Inventories.Models;
using MassTransit;
using Models;
using Products.ValueObjects;
using ValueObjects;

public static class ManualMappings
{
    public static IEnumerable<OrderItem> MapTo(this IEnumerable<ItemDto> items, OrderId id,
        IEnumerable<InventoryItems> inventoryItems)
    {
        return items?.Select(x => new OrderItem
        {
            Id = OrderItemId.Of(NewId.NextGuid()),
            Quantity = Quantity.Of(x.Quantity),
            ProductId = ProductId.Of(x.ProductId),
            Product = inventoryItems.FirstOrDefault(x => x.ProductId == ProductId.Of(x.ProductId))?.Product,
            OrderId = id
        });
    }
}
