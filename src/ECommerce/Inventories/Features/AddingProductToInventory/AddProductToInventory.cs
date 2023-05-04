namespace ECommerce.Inventories.Features.AddingProductToInventory;

using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Web;
using Data;
using Enums;
using ValueObjects;
using ECommerce.Products.ValueObjects;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Models;

public record ProductAddedToInventoryDomainEvent
    (Guid Id, Guid InventoryId, Guid ProductId, ProductStatus Status, int Quantity) : IDomainEvent;

public record ProductUpdatedToInventoryDomainEvent
    (Guid Id, Guid InventoryId, Guid ProductId, ProductStatus Status, int Quantity) : IDomainEvent;

public record AddProductToInventory(Guid InventoryId, Guid ProductId, int Quantity) : ICommand<AddProductToInventoryResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record AddProductToInventoryResult(Guid Id);

public record AddProductToInventoryRequestDto(Guid InventoryId, Guid ProductId, int Quantity);

public record AddProductToInventoryResponseDto(Guid Id);

public class AddProductToInventoryEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/inventory/add-product-to-inventory", async (
                AddProductToInventoryRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<AddProductToInventory>(request);

                var result = await mediator.Send(command, cancellationToken);

                var response = mapper.Map<AddProductToInventoryResponseDto>(result);

                return Results.Ok(response);
            })
            .WithName("Add Product To Inventory")
            .WithSummary("Add Product To Inventory")
            .WithDescription("Add Product To Inventory")
            .WithApiVersionSet(builder.NewApiVersionSet("Inventory").Build())
            .Produces<AddProductToInventoryResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class AddProductToInventoryValidator : AbstractValidator<AddProductToInventory>
{
    public AddProductToInventoryValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId must be not empty");
        RuleFor(x => x.InventoryId).NotEmpty().WithMessage("InventoryId must be not empty");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

public class AddProductToInventoryHandler : ICommandHandler<AddProductToInventory, AddProductToInventoryResult>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public AddProductToInventoryHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<AddProductToInventoryResult> Handle(AddProductToInventory request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var productInventoryItems = await _eCommerceDbContext.InventoryItems
            .SingleOrDefaultAsync(
                x => x.ProductId == ProductId.Of(request.ProductId) && x.Status == ProductStatus.InStock,
                cancellationToken: cancellationToken);

        if (productInventoryItems is not null)
        {
            productInventoryItems.UpdateProductToInventory(productInventoryItems.Id,
                InventoryId.Of(request.InventoryId),
                ProductId.Of(request.ProductId),
                Quantity.Of(request.Quantity + productInventoryItems.Quantity.Value));

            _eCommerceDbContext.InventoryItems.Update(productInventoryItems);
            return new AddProductToInventoryResult(productInventoryItems.Id.Value);
        }

        var productInventoryItemsEntity = InventoryItems.AddProductToInventory(InventoryItemsId.Of(request.Id),
            InventoryId.Of(request.InventoryId),
            ProductId.Of(request.ProductId),
            Quantity.Of(request.Quantity));

        await _eCommerceDbContext.InventoryItems.AddAsync(productInventoryItemsEntity, cancellationToken);

        return new AddProductToInventoryResult(productInventoryItemsEntity.Id.Value);
    }
}
