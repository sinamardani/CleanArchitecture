using Domain.Commons.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Application.Commons.Interfaces;

namespace Persistence.Data.Interceptors;

public class AuditTableEntityInterceptor(ICurrentUserService currentUserService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditFields(DbContext? context)
    {
        if (context == null)
            return;

        var userId = currentUserService.UserId;
        var now = DateTimeOffset.UtcNow;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is IBaseAuditTableEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

        foreach (var entry in entries)
        {
            var entity = (IBaseAuditTableEntity)entry.Entity;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedOn = now;
                    entity.CreatedBy = userId ?? 0;
                    entity.LastModifiedOn = now;
                    entity.ModifiedBy = userId;
                    entity.DeletedOn = null;
                    entity.DeletedBy = null;
                    break;

                case EntityState.Modified:
                    entry.Property(nameof(IBaseAuditTableEntity.CreatedOn)).IsModified = false;
                    entry.Property(nameof(IBaseAuditTableEntity.CreatedBy)).IsModified = false;
                    
                    entity.LastModifiedOn = now;
                    entity.ModifiedBy = userId;
                    break;

                case EntityState.Deleted:
                    var isAlreadySoftDeleted = entity.DeletedOn.HasValue || entity.DeletedBy.HasValue;
                    
                    if (isAlreadySoftDeleted)
                    {
                        break;
                    }
                    
                    entry.State = EntityState.Modified;
                    entity.DeletedOn = now;
                    entity.DeletedBy = userId;
                    break;
            }
        }
    }
}
