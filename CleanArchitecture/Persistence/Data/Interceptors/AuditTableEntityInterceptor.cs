using System.Linq.Expressions;
using Domain.Commons.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Application.Commons.Interfaces;

namespace Persistence.Data.Interceptors;

public class AuditTableEntityInterceptor(ICurrentUserService currentUserService)
    : SaveChangesInterceptor, IQueryExpressionInterceptor
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
                    entry.State = EntityState.Modified;
                    entity.DeletedOn = now;
                    entity.DeletedBy = userId;
                    break;
            }
        }
    }

    public Expression QueryCompilationStarting(
        Expression queryExpression,
        QueryExpressionEventData eventData)
    {
        return new SoftDeleteQueryExpressionVisitor().Visit(queryExpression);
    }

    private class SoftDeleteQueryExpressionVisitor : ExpressionVisitor
    {
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var visited = base.VisitMethodCall(node);

            if (visited is MethodCallExpression methodCall)
            {
                var entityType = ExtractEntityType(methodCall);
                if (entityType != null && typeof(ISoftDelete).IsAssignableFrom(entityType))
                {
                    if (!HasSoftDeleteFilter(methodCall))
                    {
                        var parameter = Expression.Parameter(entityType, "e");
                        var deletedByProperty = Expression.Property(parameter, nameof(ISoftDelete.DeletedBy));
                        var deletedOnProperty = Expression.Property(parameter, nameof(ISoftDelete.DeletedOn));
                        
                        var deletedByIsNull = Expression.Equal(deletedByProperty, Expression.Constant(null, typeof(int?)));
                        var deletedOnIsNull = Expression.Equal(deletedOnProperty, Expression.Constant(null, typeof(DateTimeOffset?)));
                        
                        var notDeletedCondition = Expression.AndAlso(deletedByIsNull, deletedOnIsNull);
                        var whereLambda = Expression.Lambda(notDeletedCondition, parameter);
                        
                        var whereMethod = typeof(Queryable).GetMethods()
                            .First(m => m.Name == nameof(Queryable.Where) && 
                                       m.GetParameters().Length == 2 &&
                                       m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>))
                            .MakeGenericMethod(entityType);
                        
                        return Expression.Call(whereMethod, methodCall, whereLambda);
                    }
                }
            }

            return visited;
        }

        private Type? ExtractEntityType(Expression expression)
        {
            while (expression is MethodCallExpression methodCall)
            {
                if (methodCall.Arguments.Count > 0)
                {
                    expression = methodCall.Arguments[0];
                }
                else
                {
                    break;
                }
            }

            if (expression is MemberExpression memberExpression)
            {
                var memberType = memberExpression.Type;
                if (memberType.IsGenericType &&
                    memberType.GetGenericTypeDefinition() == typeof(DbSet<>))
                {
                    return memberType.GetGenericArguments()[0];
                }
            }

            if (expression.Type.IsGenericType &&
                expression.Type.GetGenericTypeDefinition() == typeof(IQueryable<>))
            {
                return expression.Type.GetGenericArguments()[0];
            }

            return null;
        }

        private bool HasSoftDeleteFilter(Expression expression)
        {
            var visitor = new SoftDeleteFilterChecker();
            visitor.Visit(expression);
            return visitor.HasFilter;
        }

        private class SoftDeleteFilterChecker : ExpressionVisitor
        {
            public bool HasFilter { get; private set; }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.Name == nameof(Queryable.Where) && node.Arguments.Count == 2)
                {
                    if (node.Arguments[1] is LambdaExpression lambda)
                    {
                        var body = lambda.Body.ToString();
                        if (body.Contains(nameof(ISoftDelete.DeletedBy)) || 
                            body.Contains(nameof(ISoftDelete.DeletedOn)))
                        {
                            HasFilter = true;
                            return node;
                        }
                    }
                }

                return base.VisitMethodCall(node);
            }
        }
    }
}

