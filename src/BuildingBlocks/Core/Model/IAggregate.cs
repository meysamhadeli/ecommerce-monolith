using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Core.Model;

public interface IAggregate<out T> : IEntity<T>
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IEvent[] ClearDomainEvents();
}

public interface IAggregate : IAggregate<Guid>
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IEvent[] ClearDomainEvents();
}
