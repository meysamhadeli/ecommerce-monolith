namespace ECommerce.Orders.Models;

using BuildingBlocks.Core.Model;
using Contracts;
using Customers.Models;
using Customers.ValueObjects;
using Enums;
using ValueObjects;

public record Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = new List<OrderItem>();
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public OrderStatus Status { get; private set; }
    public TotalPrice TotalPrice { get; private set; }

    public DateTime OrderDate { get; private set; }


    public void AddItems(IEnumerable<OrderItem> items)
    {
        _orderItems.AddRange(items);
    }

    public void ApplyDiscount(DiscountType type, decimal value)
    {
        var discountStrategy = DiscountStrategyFactory.CreateDiscountStrategy(type, value);

        if (discountStrategy != null)
        {
            TotalPrice.Value -= discountStrategy.ApplyDiscount(TotalPrice.Value);
        }

        TotalPrice.Value -= TotalPrice.Value;
    }

    public (IEnumerable<OrderItem> ExpressShipmentItems, IEnumerable<OrderItem> RegularShipmentItems) ApplyShipment()
    {
        var regularItems = new List<OrderItem>();
        var expressItems = new List<OrderItem>();

        var shipmentRegularPostStrategy = ShipmentStrategyFactory.CreateShipmentStrategy(ShipmentType.RegularPost);

        if (shipmentRegularPostStrategy != null)
        {
            regularItems = shipmentRegularPostStrategy.GetOrderItemsToShip(_orderItems);
            TotalPrice.Value += shipmentRegularPostStrategy.GetShipmentPriceُ();
        }

        var shipmentExpressPostStrategy = ShipmentStrategyFactory.CreateShipmentStrategy(ShipmentType.ExpressPost);

        if (shipmentExpressPostStrategy != null)
        {
            expressItems = shipmentExpressPostStrategy.GetOrderItemsToShip(_orderItems);
            TotalPrice.Value += shipmentExpressPostStrategy.GetShipmentPriceُ();
        }

        return (expressItems, regularItems);
    }

    public void CalculateTotalPrice()
    {
        TotalPrice.Value = _orderItems.Sum(item => item.CalculatePrice());
    }
}
