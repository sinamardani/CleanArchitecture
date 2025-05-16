namespace CleanArchitecture.Core.Domain.Common.Interfaces;

public interface IBaseAuditTableEntity
{
    DateTime CreatedOn { get; set; }
    Guid CreatedBy { get; set; }

    DateTime? EditedOn { get; set; }
    Guid? EditedBy { get; set; }

    DateTime? DeletedOn { get; set; }
    Guid? DeletedBy { get; set; }

    bool IsDelete { get; set; }
}