namespace Domain.Commons.Interfaces;

public interface IBaseAuditTableEntity : ISoftDelete
{
    DateTimeOffset CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    DateTimeOffset? LastModifiedOn { get; set; }
    public int? ModifiedBy { get; set; }
}