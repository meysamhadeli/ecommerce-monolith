using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Core;

public interface IEventMapper
{
    IIntegrationEvent MapToIntegrationEvent(IDomainEvent @event);
}
