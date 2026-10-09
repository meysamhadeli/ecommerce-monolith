namespace ECommerce.Products.Features.CreatingProduct;

using Ardalis.GuardClauses;
using AutoMapper;
using Data;
using Exceptions;
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

public record CreateProduct(string Name, string Barcode, bool Weighted,
    Guid CategoryId, decimal Price, decimal ProfitMargin, string Description) : ICommand<CreateProductResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record CreateProductResult(Guid Id);

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

        var productExists = await _eCommerceDbContext.Products
            .AnyAsync(x => x.Id == request.Id, cancellationToken);

        if (productExists)
        {
            throw new ProductAlreadyExistException();
        }

        var product = new Models.Product
        {
            Id = request.Id,
            Name = request.Name,
            Barcode = request.Barcode,
            Description = request.Description,
            IsBreakable = request.Weighted,
            CategoryId = request.CategoryId,
            Price = request.Price,
            ProfitMargin = request.ProfitMargin,
            NetPrice = request.Price + request.ProfitMargin
        };

        await _eCommerceDbContext.Products.AddAsync(product, cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
