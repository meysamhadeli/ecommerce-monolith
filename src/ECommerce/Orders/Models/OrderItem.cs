namespace ECommerce.Orders.Models;

using Products.Models;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    public Guid OrderId { get; set; }

    public Order Order { get; set; }

    public int Quantity { get; set; }
}
