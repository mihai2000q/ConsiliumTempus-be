namespace ConsiliumTempus.Domain.Common.Models;

public sealed class PaginationInfo
{
    public int PageSize { get; }
    public int CurrentPage { get; }

    private PaginationInfo(int pageSize, int currentPage)
    {
        PageSize = pageSize;
        CurrentPage = currentPage;
    }

    public static PaginationInfo? Create(int? pageSize, int? currentPage)
    {
        if (pageSize is null || currentPage is null) return default;
        return new PaginationInfo(pageSize.Value, currentPage.Value);
    }

    public int GetTotalPages(int totalCount)
    {
        return (int)Math.Ceiling((decimal)totalCount / PageSize);
    }
    
    public void Deconstruct(out int pageSize, out int currentPage)
    {
        pageSize = PageSize;
        currentPage = CurrentPage;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PageSize, CurrentPage);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is PaginationInfo other && Equals(other);
    }

    private bool Equals(PaginationInfo other)
    {
        return PageSize == other.PageSize &&
               CurrentPage == other.CurrentPage;
    }
}