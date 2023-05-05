namespace ECommerce.Orders.Features.RegisteringNewOrder;

using AutoMapper;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Web;
using Data;
using Dtos;
using Enums;
using FluentValidation;
using Inventories.Enums;
using Inventories.Features.AddingProductToInventory;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using Customers.ValueObjects;
using Exceptions;
using Inventories.Models;
using Models;
using Products.ValueObjects;
using ValueObjects;

public record NewOrderRegisteredDomainEvent
    (Guid Id, Guid InventoryId, Guid ProductId, int Quantity, OrderStatus Status) : IDomainEvent;

public record OrderInitialedDomainEvent
(Guid Id, Guid CustomerId, DiscountType DiscountType, decimal DiscountValue,
    OrderStatus Status = OrderStatus.Pending, bool isDeleted = false) : IDomainEvent;

public record OrderDiscountAppliedDomainEvent
(Guid Id, Guid CustomerId, DiscountType DiscountType, decimal DiscountValue,
    OrderStatus Status, bool isDeleted = false) : IDomainEvent;

public record OrderShipmentAppliedDomainEvent
(Guid Id, Guid CustomerId, IEnumerable<OrderItemDto> RegularOrderItems, IEnumerable<OrderItemDto> ExpressOrderItems,
    OrderStatus Status, bool isDeleted = false) : IDomainEvent;

public record OrderTotalPriceAddedDomainEvent
(Guid Id, Guid CustomerId, DateTime OrderDate, decimal TotalPrice,
    OrderStatus Status, IEnumerable<OrderItemDto> OrderItems, bool isDeleted = false) : IDomainEvent;

public record OrderItemsAddedToOrderDomainEvent (IEnumerable<OrderItemDto> OrderItems) : IDomainEvent;

public record RegisterNewOrder(Guid CustomerId,
    IEnumerable<ItemDto> Items, DiscountType DiscountType, decimal DiscountValue, DateTime? OrderDate = null) : ICommand<RegisterNewOrderResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record RegisterNewOrderRequestDto(Guid CustomerId,
    IEnumerable<ItemDto> Items, DiscountType DiscountType, decimal DiscountValue, DateTime? OrderDate = null);

public record RegisterNewOrderResult(Guid Id, Guid CustomerId, string Status, decimal TotalPrice,
    DateTime OrderDate, IEnumerable<OrderItemDto> RegularOrderItems, IEnumerable<OrderItemDto> ExpressOrderItems,
    string DiscountType, decimal DiscountValue);

public record RegisterNewOrderResponseDto(Guid Id, Guid CustomerId, string Status, decimal TotalPrice,
    DateTime OrderDate, IEnumerable<OrderItemDto> RegularOrderItems, IEnumerable<OrderItemDto> ExpressOrderItems,
    string DiscountType, decimal DiscountValue);

public class RegisterNewOrderEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/order/register-new-order", async (
                RegisterNewOrderRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<RegisterNewOrder>(request);

                var result = await mediator.Send(command, cancellationToken);

                var response = mapper.Map<RegisterNewOrderResponseDto>(result);

                return Results.Ok(response);
            })
            .WithName("Register New Order")
            .WithSummary("Register New Order")
            .WithDescription("Register New Order")
            .WithApiVersionSet(builder.NewApiVersionSet("Order").Build())
            .Produces<AddProductToInventoryResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class RegisterNewOrderValidator : AbstractValidator<RegisterNewOrder>
{
    public RegisterNewOrderValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId must be not empty");
        RuleFor(x => x.Items).NotEmpty().WithMessage("Items must be not empty");
        RuleFor(x => x.Items.Count()).GreaterThan(0).WithMessage("Items must be greater than 0");
        RuleFor(x => x.DiscountValue).GreaterThanOrEqualTo(0)
            .WithMessage("DiscountValue must be equal or greater than 0");

        RuleFor(x => x.DiscountType).Must(p => (p.GetType().IsEnum &&
                                                p == DiscountType.None) ||
                                               p == DiscountType.AmountDiscount ||
                                               p == DiscountType.PercentageDiscount)
            .WithMessage("Status must be None, AmountDiscount or PercentageDiscount");
    }
}

public class RegisterNewOrderHandler : ICommandHandler<RegisterNewOrder, RegisterNewOrderResult>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public RegisterNewOrderHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<RegisterNewOrderResult> Handle(RegisterNewOrder request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var customer = await _eCommerceDbContext.Customers.FirstOrDefaultAsync(
            x => x.Id == CustomerId.Of(request.CustomerId),
            cancellationToken: cancellationToken);

        if (customer is null)
        {
            throw new CustomerNotExistException();
        }

        var inventoryItems = new List<InventoryItems>();

        foreach (var orderItem in request.Items)
        {
            var existItem =
                await _eCommerceDbContext.InventoryItems.Include(i => i.Product).FirstOrDefaultAsync(x =>
                    x.ProductId == ProductId.Of(orderItem.ProductId) && x.Status == ProductStatus.InStock &&
                    x.Quantity.Value >= orderItem.Quantity, cancellationToken: cancellationToken);

            if (existItem is null)
            {
                throw new OrderItemNotExistInInventoryException(orderItem.ProductId, orderItem.Quantity);
            }

            inventoryItems.Add(existItem);
        }

        var order = Order.Create(OrderId.Of(request.Id), customer, request.DiscountType, request.DiscountValue, OrderDate.Of(request.OrderDate ?? DateTime.Now));

        var orderItems = request.Items?.MapTo(order.Id, inventoryItems).ToList();

        order.AddItems(orderItems);

        order.CalculateTotalPrice();

        var shipmentOrderResult = order.ApplyShipment();

        order.ApplyDiscount(request.DiscountType, request.DiscountValue);

        await _eCommerceDbContext.Orders.AddAsync(order, cancellationToken);

        if (orderItems != null)
        {
            await _eCommerceDbContext.OrderItems.AddRangeAsync(orderItems, cancellationToken);
        }

        return new RegisterNewOrderResult(order.Id.Value, customer.Id.Value, order.Status.ToString(), order.TotalPrice.Value,
            order.OrderDate, shipmentOrderResult.RegularShipmentItems, shipmentOrderResult.ExpressShipmentItems,
            request.DiscountType.ToString(), request.DiscountValue);
    }
}
