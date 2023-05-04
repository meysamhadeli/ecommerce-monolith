namespace ECommerce.Categories.Features.CreatingCategory;

using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Web;
using Data;
using Exceptions;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ValueObjects;

public record CreateCategory(string Name) : ICommand<CreateCategoryResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record CreateCategoryResult(Guid Id);

public record CategoryCreatedDomainEvent(Guid Id, string Name, bool IsDeleted) : IDomainEvent;

public record CreateCategoryRequestDto(string Name);

public record CreateCategoryResponseDto(Guid Id);

public class CreateCategoryEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/catalog/category", async (CreateCategoryRequestDto request,
                IMediator mediator, IMapper mapper,
                CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<CreateCategory>(request);

                var result = await mediator.Send(command, cancellationToken);

                var response = new CreateCategoryResponseDto(result.Id);

                return Results.Ok(response);
            })
            .WithName("Create Category")
            .WithSummary("Create Category")
            .WithDescription("Create Category")
            .WithApiVersionSet(builder.NewApiVersionSet("Catalog").Build())
            .Produces<CreateCategoryResponseDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public class CreateCategoryValidator : AbstractValidator<CreateCategory>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be not empty");
    }
}

public class CreateCategoryHandler : ICommandHandler<CreateCategory, CreateCategoryResult>
{
    private readonly ECommerceDbContext _eCommerceDbContext;

    public CreateCategoryHandler(ECommerceDbContext eCommerceDbContext)
    {
        _eCommerceDbContext = eCommerceDbContext;
    }

    public async Task<CreateCategoryResult> Handle(CreateCategory request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var category = await _eCommerceDbContext.Categories.SingleOrDefaultAsync(x => x.Id == CategoryId.Of(request.Id), cancellationToken);

        if (category is not null)
        {
            throw new CategoryAlreadyExistException();
        }

        var categoryEntity = Models.Category.Create(CategoryId.Of(request.Id), Name.Of(request.Name));

        var newCategory = (await _eCommerceDbContext.Categories.AddAsync(categoryEntity, cancellationToken)).Entity;

        return new CreateCategoryResult(newCategory.Id.Value);
    }
}
