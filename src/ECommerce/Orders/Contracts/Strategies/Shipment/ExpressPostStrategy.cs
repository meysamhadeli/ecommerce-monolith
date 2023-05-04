namespace ECommerce.Orders.Contracts.Strategies.Shipment;

using ECommerce.Orders.ValueObjects;
using Models;

public class ExpressPostStrategy : IShipmentStrategy
{
    private const decimal shipmentExpressPostPrice = 500;

    public List<OrderItem> GetOrderItemsToShip(IList<OrderItem> orderItems)
    {
        return orderItems.Where(p => p.Product.IsBreakable).ToList();
    }

    public decimal GetShipmentPriceُ() => shipmentExpressPostPrice;
}
