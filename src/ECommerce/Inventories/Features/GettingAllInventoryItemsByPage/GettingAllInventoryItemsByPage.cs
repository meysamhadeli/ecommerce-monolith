namespace ECommerce.Inventories.Features.GettingAllInventoryItemsByPage;

using AutoMapper;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Pagination;
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
using Enums;
using Microsoft.EntityFrameworkCore;

public record GetAllInventoryItemsByPage
(int PageNumber, int PageSize, string Filters, string SortOrder,
    Guid? InventoryId, ProductStatus Status = 0) : IPageQuery<GetAllInventoryItemsByPageResult>;

public record GetAllInventoryItemsByPageRequestDto
    (int PageNumber, int PageSize, string Filters, string SortOrder, Guid? InventoryId, ProductStatus Status = 0) : IPageRequest;

public record GetAllInventoryItemsByPageResult(IPageList<InventoryItemsDto> InventoryItems);

public record GetAllInventoryItemsByPageResponseDto(IPageList<InventoryItemsDto> InventoryItems);

public class GetAllInventoryItemsByPageValidator : AbstractValidator<GetAllInventoryItemsByPage>
{
    public GetAllInventoryItemsByPageValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetAllInventoryItemsEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/catalog/get-all-inventory-items-by-page", async (
                [AsParameters] GetAllInventoryItemsByPageRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<GetAllInventoryItemsByPage>(request);

                var result = await mediator.Send(command, cancellationToken);

                var response = mapper.Map<GetAllInventoryItemsByPageResponseDto>(result);

                return Results.Ok(response);
            })
            .WithName("Get All Inventory Items By Page")
            .WithSummary("Get All Inventory Items By Page")
            .WithDescription("Get All Inventory Items By Page")
            .WithApiVersionSet(builder.NewApiVersionSet("Inventory").Build())
            .Produces<GetAllInventoryItemsByPageResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class
    GetAllInventoryItemsByPageHandler : IRequestHandler<GetAllInventoryItemsByPage, GetAllInventoryItemsByPageResult>
{
    private readonly ISieveProcessor _sieveProcessor;
    private readonly IMapper _mapper;
    private readonly ECommerceDbContext _eCommerceDbContext;

    public GetAllInventoryItemsByPageHandler(
        ISieveProcessor sieveProcessor,
        IMapper mapper,
        ECommerceDbContext eCommerceDbContext
    )
    {
        _sieveProcessor = sieveProcessor;
        _mapper = mapper;
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<GetAllInventoryItemsByPageResult> Handle(GetAllInventoryItemsByPage request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var inventoryItems = request?.InventoryId != null
            ? _eCommerceDbContext.InventoryItems.AsNoTracking().Where(x=>x.InventoryId == request.InventoryId)
            : _eCommerceDbContext.InventoryItems.AsNoTracking();

        inventoryItems = request.Status == ProductStatus.None
            ? inventoryItems
            : inventoryItems.Where(x => x.Status == request.Status);

        var pageList = await inventoryItems
            .ApplyPagingAsync(request, _sieveProcessor, cancellationToken);

        var result = _mapper.Map<PageList<InventoryItemsDto>>(pageList);

        return new GetAllInventoryItemsByPageResult(result);
    }
}
