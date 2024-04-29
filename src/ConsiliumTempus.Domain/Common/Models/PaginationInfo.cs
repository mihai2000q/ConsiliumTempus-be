namespace ConsiliumTempus.Domain.Common.Models;

public sealed record PaginationInfo(
    int PageSize,
    int CurrentPage)
{
    public int GetTotalPages(int totalCount)
    {
        return (int)Math.Floor((decimal)totalCount / PageSize);
    }
}