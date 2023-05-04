namespace ECommerce.Orders.Dtos;

using Enums;

public record OrderDto(Guid Id, Guid CustomerId, OrderStatus Status, decimal TotalPrice, DateTime OrderDate,
    IEnumerable<OrderItemDto> OrderItems);
