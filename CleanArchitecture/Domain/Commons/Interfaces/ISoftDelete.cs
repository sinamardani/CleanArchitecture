namespace Domain.Commons.Interfaces;

public interface ISoftDelete
{
    public DateTimeOffset? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }
}