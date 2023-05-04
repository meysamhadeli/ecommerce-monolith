using System.Security.Claims;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Core;

using global::MassTransit;

public sealed class EventDispatcher : IEventDispatcher
{
    private readonly IEventMapper _eventMapper;
    private readonly ILogger<EventDispatcher> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    readonly IPublishEndpoint _publishEndpoint;

    public EventDispatcher(IServiceScopeFactory serviceScopeFactory,
        IEventMapper eventMapper,
        ILogger<EventDispatcher> logger,
        IHttpContextAccessor httpContextAccessor,
        IPublishEndpoint publishEndpoint)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _eventMapper = eventMapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _publishEndpoint = publishEndpoint;
    }


    public async Task SendAsync<T>(IReadOnlyList<T> events, Type type = null,
        CancellationToken cancellationToken = default)
        where T : IEvent
    {
        if (events.Count > 0)
        {
            async Task PublishIntegrationEvent(IReadOnlyList<IIntegrationEvent> integrationEvents)
            {
                foreach (var integrationEvent in integrationEvents)
                {
                    await _publishEndpoint.Publish((object)integrationEvent, cancellationToken);
                }
            }

            switch (events)
            {
                case IReadOnlyList<IDomainEvent> domainEvents:
                {
                    var integrationEvents = await MapDomainEventToIntegrationEventAsync(domainEvents)
                        .ConfigureAwait(false);

                    await PublishIntegrationEvent(integrationEvents);
                    break;
                }

                case IReadOnlyList<IIntegrationEvent> integrationEvents:
                    await PublishIntegrationEvent(integrationEvents);
                    break;
            }
        }
    }

    public async Task SendAsync<T>(T @event, Type type = null,
        CancellationToken cancellationToken = default)
        where T : IEvent =>
        await SendAsync(new[] { @event }, type, cancellationToken);


    private Task<IReadOnlyList<IIntegrationEvent>> MapDomainEventToIntegrationEventAsync(
        IReadOnlyList<IDomainEvent> events)
    {
        _logger.LogTrace("Processing integration events start...");

        var integrationEvents = new List<IIntegrationEvent>();
        using var scope = _serviceScopeFactory.CreateScope();
        foreach (var @event in events)
        {
            var eventType = @event.GetType();
            _logger.LogTrace($"Handling domain event: {eventType.Name}");

            var integrationEvent = _eventMapper.MapToIntegrationEvent(@event);

            if (integrationEvent is null) continue;

            integrationEvents.Add(integrationEvent);
        }

        _logger.LogTrace("Processing integration events done...");

        return Task.FromResult<IReadOnlyList<IIntegrationEvent>>(integrationEvents);
    }

    private IDictionary<string, object> GetHeaders()
    {
        var headers = new Dictionary<string, object>();
        headers.Add("CorrelationId", _httpContextAccessor?.HttpContext?.GetCorrelationId());
        headers.Add("UserId", _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
        headers.Add("UserName", _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name));

        return headers;
    }
}
