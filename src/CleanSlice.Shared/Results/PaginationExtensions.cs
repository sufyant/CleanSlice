namespace CleanSlice.Shared.Results;

public static class PaginationExtensions
{
    public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, int page, int pageSize)
    {
        var totalCount = source.Count();
        var items = source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PagedResult<T>.Create(items, totalCount, page, pageSize);
    }

    public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, PagedRequest request)
        => source.ToPagedResult(request.Page, request.PageSize);

    public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, int page, int pageSize)
    {
        var totalCount = source.Count();
        var items = source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PagedResult<T>.Create(items, totalCount, page, pageSize);
    }

    public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, PagedRequest request)
        => source.ToPagedResult(request.Page, request.PageSize);
}
