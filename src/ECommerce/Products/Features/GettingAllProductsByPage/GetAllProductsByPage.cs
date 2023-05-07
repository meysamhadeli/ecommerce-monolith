namespace ECommerce.Products.Features.GettingAllProductsByPage;

using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.Pagination;
using BuildingBlocks.Web;
using Data;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

public record GetProductsByPage
    (int PageNumber, int PageSize, string Filters, string SortOrder) : IPageQuery<GetProductsByPageResult>;

public record GetProductsByPageRequestDto
    (int PageNumber, int PageSize, string Filters, string SortOrder) : IPageRequest;

public record GetProductsByPageResult(IPageList<ProductDto> Products);

public record GetProductsByPageResponseDto(IPageList<ProductDto> Products);

public class GetProductsByPageValidator : AbstractValidator<GetProductsByPage>
{
    public GetProductsByPageValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetProductsByPageEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/catalog/get-products-by-page", async (
                [AsParameters] GetProductsByPageRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<GetProductsByPage>(request);

                var result = await mediator.Send(command, cancellationToken);

                var response = mapper.Map<GetProductsByPageResponseDto>(result);

                return Results.Ok(response);
            })
            .WithName("Get Products By Page")
            .WithSummary("Get Products By Page")
            .WithDescription("Get Products By Page")
            .WithApiVersionSet(builder.NewApiVersionSet("Catalog").Build())
            .Produces<GetProductsByPageResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class GetProductByPageHandler : IRequestHandler<GetProductsByPage, GetProductsByPageResult>
{
    private readonly ISieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;
    private readonly ECommerceDbContext _eCommerceDbContext;

    public GetProductByPageHandler(
        ISieveProcessor sieveProcessor,
        IMapper mapper,
        ECommerceDbContext eCommerceDbContext
    )
    {
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<GetProductsByPageResult> Handle(GetProductsByPage request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var pageList = await _eCommerceDbContext.Products.AsNoTracking().ApplyPagingAsync(request, _sieveProcessor, cancellationToken);

        var result = _mapper.Map<PageList<ProductDto>>(pageList);

        return new GetProductsByPageResult(result);
    }
}
