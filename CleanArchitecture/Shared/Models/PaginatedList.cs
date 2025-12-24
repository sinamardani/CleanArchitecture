using Microsoft.EntityFrameworkCore;

namespace Shared.Models;

public class PaginatedList<T>(List<T> items, int count, int pageNumber, int pageSize)
{
    public List<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
    public int TotalCount { get; } = count;
    public int PageSize { get; } = pageSize;

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public static PaginatedList<T> Create(
        IEnumerable<T> source,
        int pageNumber,
        int pageSize)
    {
        var items = source.ToList();
        var count = items.Count;
        var pagedItems = items
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedList<T>(pagedItems, count, pageNumber, pageSize);
    }
}

