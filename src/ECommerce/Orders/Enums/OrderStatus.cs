namespace ECommerce.Orders.Enums;

public enum OrderStatus
{
    // The order is being processed, such as waiting for payment confirmation
    Pending = 1,
    // The order has been shipped
    Shipped = 2,
    // The order has been delivered to the customer
    Delivered = 3,
    // The order has been canceled
    Canceled = 4
}
