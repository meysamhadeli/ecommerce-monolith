namespace ECommerce.Products.Features.CreatingProduct;

using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Web;
using ECommerce.Categories.ValueObjects;
using ECommerce.Data;
using ECommerce.Products.Exceptions;
using ECommerce.Products.Models;
using ECommerce.Products.ValueObjects;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Name = ValueObjects.Name;

public record CreateProduct(string Name, string Barcode, bool Weighted,
    Guid CategoryId, decimal Price, decimal ProfitMargin, string Description) : ICommand<CreateProductResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record CreateProductResult(Guid Id);

public record ProductCreatedDomainEvent(Guid Id, string Name, string Barcode, bool Weighted, Guid CategoryId,
    decimal Price, decimal ProfitMargin, decimal NetPrice,
    string Description, bool IsDeleted) : IDomainEvent;

public record CreateProductRequestDto(string Name, string Barcode, bool Weighted,
    Guid CategoryId, decimal Price, decimal ProfitMargin, string Description);

public record CreateProductResponseDto(Guid Id);

public class CreateProductEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/catalog/product", async (CreateProductRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<CreateProduct>(request);

                var result = await mediator.Send(command, cancellationToken);

                var response = new CreateProductResponseDto(result.Id);

                return Results.Ok(response);
            })
            .WithName("Create Product")
            .WithSummary("Create Product")
            .WithDescription("Create Product")
            .WithApiVersionSet(builder.NewApiVersionSet("Catalog").Build())
            .Produces<CreateProductResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class CreateProductValidator : AbstractValidator<CreateProduct>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Barcode).NotEmpty().WithMessage("Barcode must be not empty");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be not empty");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId must be not empty");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be equal or greater than 0");
        RuleFor(x => x.ProfitMargin).GreaterThanOrEqualTo(0)
            .WithMessage("ProfitMargin must be equal or greater than 0");
    }
}

public class CreateProductHandler : ICommandHandler<CreateProduct, CreateProductResult>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public CreateProductHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<CreateProductResult> Handle(CreateProduct request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var product = await _eCommerceDbContext.Products.SingleOrDefaultAsync(x => x.Id == ProductId.Of(request.Id),
            cancellationToken);

        if (product is not null)
        {
            throw new ProductAlreadyExistException();
        }

        var productEntity = Product.Create(ProductId.Of(request.Id), Name.Of(request.Name),
            Barcode.Of(request.Barcode), request.Weighted, CategoryId.Of(request.CategoryId), Price.Of(request.Price),
            ProfitMargin.Of(request.ProfitMargin)
            , Description.Of(request.Description));

        var newProduct = (await _eCommerceDbContext.Products.AddAsync(productEntity, cancellationToken)).Entity;

        return new CreateProductResult(newProduct.Id.Value);
    }
}
