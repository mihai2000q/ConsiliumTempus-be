using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static void ValidateError<T>(this ErrorOr<T> error, Error expectedError)
    {
        error.IsError.Should().BeTrue();
        error.Errors.Should().HaveCount(1);
        error.FirstError.Should().Be(expectedError);
    }

    internal static bool Assert(this PaginationInfo? paginationInfo, int? pageSize, int? currentPage)
    {
        if (paginationInfo is null) return true;
        paginationInfo.PageSize.Should().Be(pageSize);
        paginationInfo.CurrentPage.Should().Be(currentPage);
        return true;
    }
}