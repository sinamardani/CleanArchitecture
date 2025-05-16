using CleanArchitecture.Core.Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Domain.Common;

/// <summary>
/// BaseAuditTableEntity with custom variable id
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class BaseAuditTableEntity<TKey> : BaseEntity<TKey>, IBaseAuditTableEntity
{
    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime CreatedOn { get; set; }

    [Required]
    public Guid CreatedBy { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? EditedOn { get; set; }
    public Guid? EditedBy { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? DeletedOn { get; set; }

    [Column(TypeName = "datetime2")]
    public Guid? DeletedBy { get; set; }

    public bool IsDelete { get; set; }
}

/// <summary>
/// BaseAuditTableEntity with int variable id
/// </summary>
public abstract class BaseAuditTableEntity : BaseAuditTableEntity<int>
{

}