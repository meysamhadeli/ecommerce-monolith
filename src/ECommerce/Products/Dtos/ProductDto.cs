namespace ECommerce.Products.Dtos;

public record ProductDto(Guid Id, string Name, string Barcode, string? Description, Guid CategoryId, bool IsBreakable,
    decimal Price, decimal ProfitMargin, decimal NetPrice);
