using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
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

    internal static bool AssertPagination(this PaginationInfo? paginationInfo, int? pageSize, int? currentPage)
    {
        if (paginationInfo is null) return true;
        paginationInfo.PageSize.Should().Be(pageSize);
        paginationInfo.CurrentPage.Should().Be(currentPage);
        return true;
    }

    internal static bool AssertOrder<TEntity>(
        this IOrder<TEntity>? order,
        string? stringOrder,
        IEnumerable<OrderProperty<TEntity>> orderProperties)
    {
        if (stringOrder is null) return order is null;
        var split = stringOrder.Split(Order<object>.Separator);

        order!.Type
            .Should()
            .Be(split[1] == Order<object>.Descending ? OrderType.Descending : OrderType.Ascending);

        return orderProperties
            .Single(op => op.Identifier == split[0])
            .PropertySelector == order.PropertySelector;
    }
}