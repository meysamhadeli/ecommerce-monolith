namespace ECommerce.Categories.Features.CreatingCategory;

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

public record CreateCategory(string Name) : ICommand<CreateCategoryResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record CreateCategoryResult(Guid Id);

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

        var categoryExists = await _eCommerceDbContext.Categories
            .AnyAsync(x => x.Id == request.Id, cancellationToken);

        if (categoryExists)
        {
            throw new CategoryAlreadyExistException();
        }

        var category = new Models.Category
        {
            Id = request.Id,
            Name = request.Name
        };

        await _eCommerceDbContext.Categories.AddAsync(category, cancellationToken);

        return new CreateCategoryResult(category.Id);
    }
}
