namespace ECommerce.Orders.Models;

using BuildingBlocks.Core.Model;
using ECommerce.Products.Models;
using ECommerce.Products.ValueObjects;
using ValueObjects;

public record OrderItem : Aggregate<OrderItemId>
{
    public ProductId ProductId { get; private set; }
    public Product Product { get; private set; }
    public OrderId OrderId { get; private set; }
    public Order Order { get; private set; }
    public Quantity Quantity { get; private set; }

    public decimal CalculatePrice()
    {
        return Product.NetPrice.Value * Quantity.Value;
    }
}
