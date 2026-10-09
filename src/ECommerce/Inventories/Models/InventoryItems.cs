namespace ECommerce.Inventories.Models;

using Enums;
using Products.Models;

public class InventoryItems
{
    public Guid Id { get; set; }

    public Guid InventoryId { get; set; }

    public Inventory Inventory { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    public int Quantity { get; set; }

    public ProductStatus Status { get; set; }
}
