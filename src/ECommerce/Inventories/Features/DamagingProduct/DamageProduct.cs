namespace ECommerce.Inventories.Features.DamagingProduct;

using Ardalis.GuardClauses;
using AutoMapper;
using Data;
using Enums;
using Exceptions;
using FluentValidation;
using Griffin.Core.CQRS;
using Griffin.Web;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Models;

public record DamageProduct(Guid ProductId, int Quantity) : ICommand;

public record DamageProductRequestDto(Guid ProductId, int Quantity);

public class DamageProductEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/inventory/damage-product", async (
                DamageProductRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<DamageProduct>(request);

                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .WithName("Damage Product")
            .WithSummary("Damage Product")
            .WithDescription("Damage Product")
            .WithApiVersionSet(builder.NewApiVersionSet("Inventory").Build())
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .HasApiVersion(1.0);

        return builder;
    }
}

public class DamageProductValidator : AbstractValidator<DamageProduct>
{
    public DamageProductValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId must be not empty");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

public class DamageProductHandler : ICommandHandler<DamageProduct>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public DamageProductHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<Unit> Handle(DamageProduct request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var productsInventoryItems = await _eCommerceDbContext.InventoryItems
            .SingleOrDefaultAsync(
                x => x.ProductId == request.ProductId && x.Status == ProductStatus.InStock,
                cancellationToken: cancellationToken);

        if (productsInventoryItems is null)
        {
            throw new ProductNotExistToInventoryException();
        }

        if (request.Quantity > productsInventoryItems.Quantity)
        {
            throw new OutOfRangeQuantityException(request.Quantity, productsInventoryItems.Quantity);
        }

        productsInventoryItems.Quantity -= request.Quantity;

        _eCommerceDbContext.InventoryItems.Update(productsInventoryItems);

        var damagedInventoryItem = new InventoryItems
        {
            Id = NewId.NextGuid(),
            InventoryId = productsInventoryItems.InventoryId,
            ProductId = productsInventoryItems.ProductId,
            Quantity = request.Quantity,
            Status = ProductStatus.Damaged
        };

        await _eCommerceDbContext.InventoryItems.AddAsync(damagedInventoryItem, cancellationToken);

        return Unit.Value;
    }
}
