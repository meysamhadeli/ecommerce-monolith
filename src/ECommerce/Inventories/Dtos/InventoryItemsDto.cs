namespace ECommerce.Inventories.Dtos;

using Enums;

public record InventoryItemsDto(Guid Id, Guid InventoryId, Guid ProductId, int Quantity, ProductStatus Status);
