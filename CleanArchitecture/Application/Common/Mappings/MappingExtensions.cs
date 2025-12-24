using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
        where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(
            queryable.AsNoTracking(),
            pageNumber,
            pageSize,
            cancellationToken);

    public static Task<List<TDestination>> ProjectToListAsync<TSource, TDestination>(
        this IQueryable<TSource> queryable,
        CancellationToken cancellationToken = default)
        where TSource : class
        where TDestination : class
        => queryable
            .AsNoTracking()
            .ProjectToType<TDestination>()
            .ToListAsync(cancellationToken);
}

