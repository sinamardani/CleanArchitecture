namespace CleanArchitecture.Core.Domain.Common.Interfaces;

public interface IBaseEntity<TKey>
{
    TKey Id { get; set; }
    void AddDomainEvent(BaseEvent domainEvent);
    void RemoveDomainEvent(BaseEvent domainEvent);
    void ClearDomainEvents();
}