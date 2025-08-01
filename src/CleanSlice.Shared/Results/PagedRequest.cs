namespace CleanSlice.Shared.Results;

public class PagedRequest
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    public int Page { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;

    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;

    public static PagedRequest Create(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, bool sortDescending = false)
    {
        return new PagedRequest
        {
            Page = page,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            SortBy = sortBy,
            SortDescending = sortDescending
        };
    }
}
