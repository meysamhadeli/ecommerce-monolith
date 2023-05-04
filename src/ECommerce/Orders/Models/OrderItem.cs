namespace ECommerce.Orders.Models;

using BuildingBlocks.Core.Model;
using ECommerce.Products.Models;
using ECommerce.Products.ValueObjects;
using ValueObjects;

public record OrderItem : Entity<OrderItemId>
{
    public ProductId ProductId { get; init; }
    public Product Product { get; init; }
    public OrderId OrderId { get; init; }
    public Order Order { get; init; }
    public Quantity Quantity { get; init; }

    public decimal CalculatePrice()
    {
        return Product.NetPrice.Value * Quantity.Value;
    }
}
