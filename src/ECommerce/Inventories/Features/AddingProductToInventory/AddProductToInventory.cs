namespace ECommerce.Inventories.Features.AddingProductToInventory;

using Ardalis.GuardClauses;
using AutoMapper;
using Data;
using Enums;
using FluentValidation;
using Griffin.Core.CQRS;
using Griffin.Core.Event;
using Griffin.Web;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Models;

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

        var existingInventoryItem = await _eCommerceDbContext.InventoryItems
            .SingleOrDefaultAsync(
                x => x.ProductId == request.ProductId && x.Status == ProductStatus.InStock,
                cancellationToken: cancellationToken);

        if (existingInventoryItem is not null)
        {
            existingInventoryItem.Quantity += request.Quantity;

            _eCommerceDbContext.InventoryItems.Update(existingInventoryItem);

            return new AddProductToInventoryResult(existingInventoryItem.Id);
        }

        var inventoryItem = new InventoryItems
        {
            Id = request.Id,
            InventoryId = request.InventoryId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            Status = ProductStatus.InStock
        };

        await _eCommerceDbContext.InventoryItems.AddAsync(inventoryItem, cancellationToken);

        return new AddProductToInventoryResult(inventoryItem.Id);
    }
}
