namespace ECommerce.Categories.Features.GettingAllCategoriesByPage;

using AutoMapper;
using BuildingBlocks.Web;
using Data;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sieve.Services;
using Ardalis.GuardClauses;
using BuildingBlocks.Core.Pagination;
using Microsoft.EntityFrameworkCore;

public record GetCategoriesByPage
    (int PageNumber, int PageSize, string Filters, string SortOrder) : IPageQuery<GetCategoriesByPageResult>;

public record GetCategoriesByPageRequestDto
    (int PageNumber, int PageSize, string Filters, string SortOrder) : IPageRequest;

public record GetCategoriesByPageResult(IPageList<CategoryDto> Categories);

public record GetCategoriesByPageResponseDto(IPageList<CategoryDto> Categories);

public class GetCategoriesByPageValidator : AbstractValidator<GetCategoriesByPage>
{
    public GetCategoriesByPageValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetCategoriesEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/catalog/get-categories-by-page", async (
                [AsParameters] GetCategoriesByPageRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<GetCategoriesByPage>(request);

                var result = await mediator.Send(command, cancellationToken);

                var response = mapper.Map<GetCategoriesByPageResponseDto>(result);

                return Results.Ok(response);
            })
            .WithName("Get Categories By Page")
            .WithSummary("Get Categories By Page")
            .WithDescription("Get Categories By Page")
            .WithApiVersionSet(builder.NewApiVersionSet("Catalog").Build())
            .Produces<GetCategoriesByPageResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class GetCategoriesByPageHandler : IRequestHandler<GetCategoriesByPage, GetCategoriesByPageResult>
{
    private readonly ISieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;
    private readonly ECommerceDbContext _eCommerceDbContext;

    public GetCategoriesByPageHandler(
        ISieveProcessor sieveProcessor,
        IMapper mapper,
        ECommerceDbContext eCommerceDbContext
    )
    {
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<GetCategoriesByPageResult> Handle(GetCategoriesByPage request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var pageList = await _eCommerceDbContext.Categories.AsNoTracking().ApplyPagingAsync(request, _sieveProcessor, cancellationToken);

        var result = _mapper.Map<PageList<CategoryDto>>(pageList);

        return new GetCategoriesByPageResult(result);
    }
}
