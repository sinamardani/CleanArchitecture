using Domain.Commons.Interfaces;

namespace Domain.Commons;

public abstract class BaseAuditTableEntity<TKey> : BaseEntity<TKey>, IBaseAuditTableEntity
{
    public DateTimeOffset DeletedOn { get; set; }
    public int DeletedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public int ModifiedBy { get; set; }
}

public abstract class BaseAuditTableEntity : BaseAuditTableEntity<int>
{

}