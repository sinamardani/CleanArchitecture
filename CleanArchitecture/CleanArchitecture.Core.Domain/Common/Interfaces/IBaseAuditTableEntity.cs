namespace CleanArchitecture.Core.Domain.Common.Interfaces;

public interface IBaseAuditTableEntity
{
    DateTime CreatedOn { get; set; }
    int CreatedBy { get; set; }

    DateTime? EditedOn { get; set; }
    int? EditedBy { get; set; }

    DateTime? DeletedOn { get; set; }
    int? DeletedBy { get; set; }

    bool IsDelete { get; set; }
}