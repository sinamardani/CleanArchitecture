namespace Domain.Commons.Interfaces;

public interface IBaseEntity<TKey>
{
    TKey Id { get; set; }
}