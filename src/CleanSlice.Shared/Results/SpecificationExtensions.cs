using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Shared.Results;

public static class SpecificationExtensions
{
    public static IQueryable<T> Where<T>(this IQueryable<T> source, ISpecification<T> specification)
    {
        return source.Where(specification.ToExpression());
    }

    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        return source.Where(specification.ToExpression().Compile());
    }

    public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, ISpecification<T> specification, int page, int pageSize)
    {
        return source.Where(specification).ToPagedResult(page, pageSize);
    }

    public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, ISpecification<T> specification, PagedRequest request)
    {
        return source.Where(specification).ToPagedResult(request);
    }
}
