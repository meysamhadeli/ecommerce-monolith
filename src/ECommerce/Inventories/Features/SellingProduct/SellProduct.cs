namespace ECommerce.Inventories.Features.SellingProduct;

using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Web;
using Data;
using Enums;
using Exceptions;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Models;
using Orders.Features.RegisteringNewOrder;
using Products.ValueObjects;
using ValueObjects;

public record ProductSoldDomainEvent
    (Guid Id, Guid InventoryId, Guid ProductId, ProductStatus Status, int Quantity) : IDomainEvent;

public record SellProduct(Guid ProductId, int Quantity) : ICommand;

public record SellProductRequestDto(Guid ProductId, int Quantity);

public class SellProductEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/inventory/sell-product", async (
                SellProductRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<SellProduct>(request);

                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .WithName("Sell Product")
            .WithSummary("Sell Product")
            .WithDescription("Sell Product")
            .WithApiVersionSet(builder.NewApiVersionSet("Inventory").Build())
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class SellProductValidator : AbstractValidator<SellProduct>
{
    public SellProductValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId must be not empty");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

public class SellProductHandler : ICommandHandler<SellProduct>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public SellProductHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<Unit> Handle(SellProduct request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var productsInventoryItems = await _eCommerceDbContext.InventoryItems
            .SingleOrDefaultAsync(
                x => x.ProductId == ProductId.Of(request.ProductId) && x.Status == ProductStatus.InStock,
                cancellationToken: cancellationToken);

        if (productsInventoryItems is null)
        {
            throw new ProductNotExistToInventoryException();
        }

        if (request.Quantity > productsInventoryItems.Quantity.Value)
        {
            throw new OutOfRangeQuantityException(request.Quantity, productsInventoryItems.Quantity.Value);
        }

        productsInventoryItems.SellProduct(productsInventoryItems.Id, productsInventoryItems.InventoryId,
            ProductId.Of(request.ProductId), Quantity.Of(request.Quantity));

        _eCommerceDbContext.InventoryItems.Update(productsInventoryItems);

        return Unit.Value;
    }


    public class UpdateInventoryWhenOrderItemsAddedToOrderDomainEventHandler : INotificationHandler<OrderItemsAddedToOrderDomainEvent>
    {
        private readonly ECommerceDbContext _eCommerceDbContext;

        public UpdateInventoryWhenOrderItemsAddedToOrderDomainEventHandler(ECommerceDbContext eCommerceDbContext)
        {
            _eCommerceDbContext = eCommerceDbContext;
        }

        public async Task Handle(OrderItemsAddedToOrderDomainEvent notification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(notification, nameof(notification));

            if (notification.OrderItems.Any())
            {
                foreach (var notificationOrderItem in notification.OrderItems)
                {
                    var productsInventoryItems = await _eCommerceDbContext.InventoryItems
                        .SingleOrDefaultAsync(
                            x => x.ProductId == ProductId.Of(notificationOrderItem.ProductId) && x.Status == ProductStatus.InStock,
                            cancellationToken: cancellationToken);

                    if (productsInventoryItems is null)
                    {
                        throw new ProductNotExistToInventoryException();
                    }

                    if (notificationOrderItem.Quantity > productsInventoryItems.Quantity.Value)
                    {
                        throw new OutOfRangeQuantityException(notificationOrderItem.Quantity, productsInventoryItems.Quantity.Value);
                    }

                    productsInventoryItems.SellProduct(productsInventoryItems.Id, productsInventoryItems.InventoryId,
                        ProductId.Of(notificationOrderItem.ProductId), Quantity.Of(notificationOrderItem.Quantity));

                    _eCommerceDbContext.InventoryItems.Update(productsInventoryItems);
                }
            }

            await _eCommerceDbContext.ExecuteTransactionalAsync(cancellationToken);
        }
    }

    public class ProductSoldDomainEventHandler : INotificationHandler<ProductSoldDomainEvent>
    {
        private readonly ECommerceDbContext _eCommerceDbContext;

        public ProductSoldDomainEventHandler(ECommerceDbContext eCommerceDbContext)
        {
            _eCommerceDbContext = eCommerceDbContext;
        }

        public async Task Handle(ProductSoldDomainEvent notification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(notification, nameof(notification));

            var productInventoryItemsEntity = InventoryItems.AddProductToInventory(
                InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(notification.InventoryId),
                ProductId.Of(notification.ProductId),
                Quantity.Of(notification.Quantity),
                ProductStatus.Sold);

            await _eCommerceDbContext.InventoryItems.AddAsync(productInventoryItemsEntity, cancellationToken);
            await _eCommerceDbContext.ExecuteTransactionalAsync(cancellationToken);
        }
    }
}
