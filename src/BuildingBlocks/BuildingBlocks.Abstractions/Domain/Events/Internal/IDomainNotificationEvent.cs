namespace BuildingBlocks.Abstractions.Domain.Events.Internal;

public interface IDomainNotificationEvent<TDomainEventType> : IDomainNotificationEvent
    where TDomainEventType : IDomainEvent
{
    TDomainEventType DomainEvent { get; set; }
}

public interface IDomainNotificationEvent : IEvent { }
