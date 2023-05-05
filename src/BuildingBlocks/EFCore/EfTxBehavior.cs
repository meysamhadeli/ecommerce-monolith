using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.EFCore;

public class EfTxBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<EfTxBehavior<TRequest, TResponse>> _logger;
    private readonly IDbContext _dbContextBase;
    private readonly IMediator _mediator;

    public EfTxBehavior(
        ILogger<EfTxBehavior<TRequest, TResponse>> logger,
        IDbContext dbContextBase,
        IMediator mediator
    )
    {
        _logger = logger;
        _dbContextBase = dbContextBase;
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{Prefix} Handled command {MediatrRequest}",
            nameof(EfTxBehavior<TRequest, TResponse>),
            typeof(TRequest).FullName);

        _logger.LogDebug(
            "{Prefix} Handled command {MediatrRequest} with content {RequestContent}",
            nameof(EfTxBehavior<TRequest, TResponse>),
            typeof(TRequest).FullName,
            JsonSerializer.Serialize(request));

        _logger.LogInformation(
            "{Prefix} Open the transaction for {MediatrRequest}",
            nameof(EfTxBehavior<TRequest, TResponse>),
            typeof(TRequest).FullName);


        var response = await next();

        _logger.LogInformation(
            "{Prefix} Executed the {MediatrRequest} request",
            nameof(EfTxBehavior<TRequest, TResponse>),
            typeof(TRequest).FullName);

        while (true)
        {
            var domainEvents = _dbContextBase.GetDomainEvents();

            if (domainEvents is null || !domainEvents.Any())
                return response;

            await _dbContextBase.ExecuteTransactionalAsync(cancellationToken);

            foreach (var @event in domainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
        }
    }
}
