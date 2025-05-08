using CleanArchitecture.Core.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Domain.Common;


/// <summary>
/// BaseEntity with custom variable id 
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

/// <summary>
/// BaseEntity with int variable id
/// </summary>
public abstract class BaseEntity : BaseEntity<int>
{

}