namespace ECommerce.Orders.Dtos;

public record OrderItemDto(Guid Id, Guid ProductId, Guid OrderId, int Quantity);
