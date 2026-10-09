namespace ECommerce.Orders.Features.RegisteringNewOrder;

using Ardalis.GuardClauses;
using AutoMapper;
using Data;
using Dtos;
using Enums;
using Exceptions;
using FluentValidation;
using Griffin.Core.CQRS;
using Griffin.Web;
using Inventories.Enums;
using Inventories.Features.AddingProductToInventory;
using Inventories.Models;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Models;

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

        RuleFor(x => x.DiscountType).Must(p => p == DiscountType.None ||
                                              p == DiscountType.AmountDiscount ||
                                              p == DiscountType.PercentageDiscount)
            .WithMessage("Status must be None, AmountDiscount or PercentageDiscount");
    }
}

public class RegisterNewOrderHandler : ICommandHandler<RegisterNewOrder, RegisterNewOrderResult>
{
    private const decimal RegularPostPrice = 200;
    private const decimal ExpressPostPrice = 500;
    private const decimal MinimumTotalPrice = 50000;

    private readonly ECommerceDbContext _eCommerceDbContext;

    public RegisterNewOrderHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<RegisterNewOrderResult> Handle(RegisterNewOrder request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var customer = await _eCommerceDbContext.Customers.FirstOrDefaultAsync(
            x => x.Id == request.CustomerId,
            cancellationToken: cancellationToken);

        if (customer is null)
        {
            throw new CustomerNotExistException();
        }

        var order = new Order
        {
            Id = request.Id,
            CustomerId = customer.Id,
            Customer = customer,
            Status = OrderStatus.Pending,
            TotalPrice = 0,
            OrderDate = request.OrderDate ?? DateTime.Now
        };

        foreach (var item in request.Items)
        {
            var inventoryItem = await _eCommerceDbContext.InventoryItems
                .Include(i => i.Product)
                .FirstOrDefaultAsync(x => x.ProductId == item.ProductId
                                          && x.Status == ProductStatus.InStock
                                          && x.Quantity >= item.Quantity,
                    cancellationToken: cancellationToken);

            if (inventoryItem is null)
            {
                throw new OrderItemNotExistInInventoryException(item.ProductId, item.Quantity);
            }

            order.OrderItems.Add(new OrderItem
            {
                Id = NewId.NextGuid(),
                OrderId = order.Id,
                ProductId = item.ProductId,
                Product = inventoryItem.Product,
                Quantity = item.Quantity
            });

            // Reduce the in-stock quantity and record the sold items in a separate inventory row.
            inventoryItem.Quantity -= item.Quantity;

            await _eCommerceDbContext.InventoryItems.AddAsync(new InventoryItems
            {
                Id = NewId.NextGuid(),
                InventoryId = inventoryItem.InventoryId,
                ProductId = inventoryItem.ProductId,
                Quantity = item.Quantity,
                Status = ProductStatus.Sold
            }, cancellationToken);
        }

        order.TotalPrice = order.OrderItems.Sum(x => x.Product.NetPrice * x.Quantity);

        if (order.TotalPrice < MinimumTotalPrice)
        {
            throw new InvalidTotalPriceRangeException(order.TotalPrice);
        }

        var regularItems = order.OrderItems.Where(x => !x.Product.IsBreakable).ToList();
        var expressItems = order.OrderItems.Where(x => x.Product.IsBreakable).ToList();

        if (regularItems.Count > 0)
        {
            order.TotalPrice += RegularPostPrice;
        }

        if (expressItems.Count > 0)
        {
            order.TotalPrice += ExpressPostPrice;
        }

        order.TotalPrice = ApplyDiscount(order.TotalPrice, request.DiscountType, request.DiscountValue);

        await _eCommerceDbContext.Orders.AddAsync(order, cancellationToken);

        return new RegisterNewOrderResult(order.Id, customer.Id, order.Status.ToString(), order.TotalPrice,
            order.OrderDate, regularItems.Select(ToDto), expressItems.Select(ToDto),
            request.DiscountType.ToString(), request.DiscountValue);
    }

    private static decimal ApplyDiscount(decimal amount, DiscountType discountType, decimal discountValue)
    {
        return discountType switch
        {
            DiscountType.AmountDiscount => amount - (amount >= discountValue ? discountValue : 0),
            DiscountType.PercentageDiscount => amount - (amount * discountValue / 100),
            _ => amount
        };
    }

    private static OrderItemDto ToDto(OrderItem orderItem)
    {
        return new OrderItemDto(orderItem.Id, orderItem.ProductId, orderItem.OrderId, orderItem.Quantity);
    }
}
