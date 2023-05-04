namespace ECommerce.Orders.Features;

using AutoMapper;
using Dtos;
using Models;
using RegisteringNewOrder;

public class OrderMappings : Profile
{
    public OrderMappings()
    {
        CreateMap<RegisterNewOrderRequestDto, RegisterNewOrder>();
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItem>();
        CreateMap<RegisterNewOrderResult, RegisterNewOrderResponseDto>();
    }
}

