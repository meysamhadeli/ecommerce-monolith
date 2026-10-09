namespace ECommerce.Products.Models;

using Categories.Models;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Barcode { get; set; }

    public string? Description { get; set; }

    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    public bool IsBreakable { get; set; }

    public decimal Price { get; set; }

    public decimal ProfitMargin { get; set; }

    public decimal NetPrice { get; set; }
}
