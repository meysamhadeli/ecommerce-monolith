namespace ECommerce.Orders.Models;

using Customers.Models;
using Enums;

public class Order
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; }

    public OrderStatus Status { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime OrderDate { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();
}
