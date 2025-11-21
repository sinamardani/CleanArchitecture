namespace Domain.Commons.Interfaces;

public interface IBaseEntity<TKey>
{
    TKey Id { get; set; }
    void AddDomainEvent(BaseEvent domainEvent);
    void RemoveDomainEvent(BaseEvent domainEvent);
    void ClearDomainEvents();
}