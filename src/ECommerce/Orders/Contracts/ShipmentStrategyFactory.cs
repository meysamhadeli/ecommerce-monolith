namespace ECommerce.Orders.Contracts;

using Enums;
using Strategies;
using Strategies.Shipment;

public static class ShipmentStrategyFactory
{
    public static IShipmentStrategy CreateShipmentStrategy(ShipmentType type)
    {
        switch (type)
        {
            case ShipmentType.RegularPost:
                return new RegularPostStrategy();
            case ShipmentType.ExpressPost:
                return new ExpressPostStrategy();
            default:
                return null;
        }
    }
}
