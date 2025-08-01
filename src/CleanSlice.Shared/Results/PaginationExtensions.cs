using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Shared.Results;

/// <summary>
/// Extension methods for pagination operations on collections and queries.
/// Provides both synchronous and asynchronous pagination capabilities.
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Converts an IEnumerable to a PagedResult with specified page and page size.
    /// Use this for in-memory collections that need pagination.
    /// 
    /// Example:
    /// var users = await userRepository.GetAllAsync();
    /// var pagedUsers = users.ToPagedResult(page: 1, pageSize: 10);
    /// </summary>
    /// <typeparam name="T">The type of items in the collection</typeparam>
    /// <param name="source">The source collection</param>
    /// <param name="page">The page number (1-based)</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <returns>A PagedResult containing the paginated items</returns>
    public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, int page, int pageSize)
    {
        var totalCount = source.Count();
        var items = source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PagedResult<T>.Create(items, totalCount, page, pageSize);
    }

    /// <summary>
    /// Converts an IEnumerable to a PagedResult using a PagedRequest object.
    /// Use this when you have a PagedRequest with search terms and sorting options.
    /// 
    /// Example:
    /// var request = PagedRequest.Create(page: 1, pageSize: 10, searchTerm: "john");
    /// var pagedUsers = users.ToPagedResult(request);
    /// </summary>
    /// <typeparam name="T">The type of items in the collection</typeparam>
    /// <param name="source">The source collection</param>
    /// <param name="request">The pagination request containing page, pageSize, and search options</param>
    /// <returns>A PagedResult containing the paginated items</returns>
    public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, PagedRequest request)
        => source.ToPagedResult(request.Page, request.PageSize);

    /// <summary>
    /// Converts an IQueryable to a PagedResult with specified page and page size.
    /// Use this for database queries that need pagination.
    /// 
    /// Example:
    /// var query = dbContext.Users.Where(u => u.IsActive);
    /// var pagedUsers = query.ToPagedResult(page: 1, pageSize: 10);
    /// </summary>
    /// <typeparam name="T">The type of items in the query</typeparam>
    /// <param name="source">The source query</param>
    /// <param name="page">The page number (1-based)</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <returns>A PagedResult containing the paginated items</returns>
    public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, int page, int pageSize)
    {
        var totalCount = source.Count();
        var items = source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PagedResult<T>.Create(items, totalCount, page, pageSize);
    }

    /// <summary>
    /// Converts an IQueryable to a PagedResult using a PagedRequest object.
    /// Use this when you have a PagedRequest with search terms and sorting options.
    /// 
    /// Example:
    /// var request = PagedRequest.Create(page: 1, pageSize: 10, searchTerm: "john");
    /// var query = dbContext.Users.Where(u => u.IsActive);
    /// var pagedUsers = query.ToPagedResult(request);
    /// </summary>
    /// <typeparam name="T">The type of items in the query</typeparam>
    /// <param name="source">The source query</param>
    /// <param name="request">The pagination request containing page, pageSize, and search options</param>
    /// <returns>A PagedResult containing the paginated items</returns>
    public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, PagedRequest request)
        => source.ToPagedResult(request.Page, request.PageSize);

    /// <summary>
    /// Converts an IQueryable to a PagedResult asynchronously with specified page and page size.
    /// Use this for database queries that need async pagination (recommended for EF Core).
    /// 
    /// Example:
    /// var query = dbContext.Users.Where(u => u.IsActive);
    /// var pagedUsers = await query.ToPagedResultAsync(page: 1, pageSize: 10, cancellationToken);
    /// </summary>
    /// <typeparam name="T">The type of items in the query</typeparam>
    /// <param name="source">The source query</param>
    /// <param name="page">The page number (1-based)</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>A PagedResult containing the paginated items</returns>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return PagedResult<T>.Create(items, totalCount, page, pageSize);
    }

    /// <summary>
    /// Converts an IQueryable to a PagedResult asynchronously using a PagedRequest object.
    /// Use this for database queries with search and sorting options (recommended for EF Core).
    /// 
    /// Example:
    /// var request = PagedRequest.Create(page: 1, pageSize: 10, searchTerm: "john");
    /// var query = dbContext.Users.Where(u => u.IsActive);
    /// var pagedUsers = await query.ToPagedResultAsync(request, cancellationToken);
    /// </summary>
    /// <typeparam name="T">The type of items in the query</typeparam>
    /// <param name="source">The source query</param>
    /// <param name="request">The pagination request containing page, pageSize, and search options</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>A PagedResult containing the paginated items</returns>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> source, PagedRequest request, CancellationToken cancellationToken = default)
        => await source.ToPagedResultAsync(request.Page, request.PageSize, cancellationToken);

    /// <summary>
    /// Converts an IEnumerable to a PagedResult asynchronously with specified page and page size.
    /// Use this for in-memory collections that need async pagination (wrapped in Task).
    /// 
    /// Example:
    /// var users = await userRepository.GetAllAsync();
    /// var pagedUsers = await users.ToPagedResultAsync(page: 1, pageSize: 10, cancellationToken);
    /// </summary>
    /// <typeparam name="T">The type of items in the collection</typeparam>
    /// <param name="source">The source collection</param>
    /// <param name="page">The page number (1-based)</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>A PagedResult containing the paginated items</returns>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IEnumerable<T> source, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = source.Count();
        var items = source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return await Task.FromResult(PagedResult<T>.Create(items, totalCount, page, pageSize));
    }

    /// <summary>
    /// Converts an IEnumerable to a PagedResult asynchronously using a PagedRequest object.
    /// Use this for in-memory collections with search and sorting options (wrapped in Task).
    /// 
    /// Example:
    /// var request = PagedRequest.Create(page: 1, pageSize: 10, searchTerm: "john");
    /// var users = await userRepository.GetAllAsync();
    /// var pagedUsers = await users.ToPagedResultAsync(request, cancellationToken);
    /// </summary>
    /// <typeparam name="T">The type of items in the collection</typeparam>
    /// <param name="source">The source collection</param>
    /// <param name="request">The pagination request containing page, pageSize, and search options</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>A PagedResult containing the paginated items</returns>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IEnumerable<T> source, PagedRequest request, CancellationToken cancellationToken = default)
        => await source.ToPagedResultAsync(request.Page, request.PageSize, cancellationToken);

    /// <summary>
    /// Applies pagination to an IQueryable using a PagedRequest object.
    /// Use this when you want to apply pagination but handle the counting separately.
    /// 
    /// Example:
    /// var query = dbContext.Users.Where(u => u.IsActive);
    /// var request = PagedRequest.Create(page: 1, pageSize: 10);
    /// var paginatedQuery = query.ApplyPagination(request);
    /// var items = await paginatedQuery.ToListAsync();
    /// </summary>
    /// <typeparam name="T">The type of items in the query</typeparam>
    /// <param name="source">The source query</param>
    /// <param name="request">The pagination request</param>
    /// <returns>A query with pagination applied</returns>
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source, PagedRequest request)
        => source.Skip(request.Skip).Take(request.Take);

    /// <summary>
    /// Applies pagination to an IQueryable with specified page and page size.
    /// Use this when you want to apply pagination but handle the counting separately.
    /// 
    /// Example:
    /// var query = dbContext.Users.Where(u => u.IsActive);
    /// var paginatedQuery = query.ApplyPagination(page: 1, pageSize: 10);
    /// var items = await paginatedQuery.ToListAsync();
    /// </summary>
    /// <typeparam name="T">The type of items in the query</typeparam>
    /// <param name="source">The source query</param>
    /// <param name="page">The page number (1-based)</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <returns>A query with pagination applied</returns>
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source, int page, int pageSize)
        => source.Skip((page - 1) * pageSize).Take(pageSize);

    /// <summary>
    /// Applies pagination to an IQueryable using a PagedRequest object.
    /// Use this when you want to apply pagination but handle the counting separately.
    /// 
    /// Example:
    /// var query = dbContext.Users.Where(u => u.IsActive);
    /// var request = PagedRequest.Create(page: 1, pageSize: 10);
    /// var paginatedQuery = query.ApplyPaginationAndSorting(request);
    /// var items = await paginatedQuery.ToListAsync();
    /// </summary>
    /// <typeparam name="T">The type of items in the query</typeparam>
    /// <param name="source">The source query</param>
    /// <param name="request">The pagination request</param>
    /// <returns>A query with pagination applied</returns>
    public static IQueryable<T> ApplyPaginationAndSorting<T>(this IQueryable<T> source, PagedRequest request)
        => source.ApplyPagination(request);
}
