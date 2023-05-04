namespace ECommerce.Orders.Contracts.Strategies.Shipment;

using ECommerce.Orders.ValueObjects;
using Models;

public class RegularPostStrategy : IShipmentStrategy
{
    private const decimal shipmentRegularPostPrice = 200;

    public List<OrderItem> GetOrderItemsToShip(IList<OrderItem> orderItems)
    {
        return orderItems.Where(p => !p.Product.IsBreakable).ToList();
    }

    public decimal GetShipmentPriceُ()
    {
        return shipmentRegularPostPrice;
    }
}
