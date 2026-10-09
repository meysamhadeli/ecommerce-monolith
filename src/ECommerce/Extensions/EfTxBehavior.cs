namespace ECommerce.Extensions;

using System.Text.Json;
using Griffin.EFCore;
using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// Wraps each request in a database transaction and persists the changes made by the handler.
/// </summary>
public class EfTxBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<EfTxBehavior<TRequest, TResponse>> _logger;

    public EfTxBehavior(IDbContext dbContext, ILogger<EfTxBehavior<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "{Prefix} Handled command {MediatrRequest} with content {RequestContent}",
            nameof(EfTxBehavior<TRequest, TResponse>),
            typeof(TRequest).FullName,
            JsonSerializer.Serialize(request));

        var response = await next();

        await _dbContext.ExecuteTransactionalAsync(cancellationToken);

        _logger.LogInformation(
            "{Prefix} Executed the {MediatrRequest} request",
            nameof(EfTxBehavior<TRequest, TResponse>),
            typeof(TRequest).FullName);

        return response;
    }
}
