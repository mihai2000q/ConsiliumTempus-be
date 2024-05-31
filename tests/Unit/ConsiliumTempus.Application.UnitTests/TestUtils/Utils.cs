using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    private static readonly TimeSpan TimeSpanPrecision = 15.Seconds();

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

    internal static bool AssertOrders<TEntity>(
        this IReadOnlyList<IOrder<TEntity>> orders,
        string[]? stringOrders,
        IEnumerable<OrderProperty<TEntity>> orderProperties)
    {
        if (stringOrders is null) return orders.Count == 0;
        return orders
            .Zip(stringOrders)
            .All(x => x.First.AssertOrder(x.Second, orderProperties));
    }

    private static bool AssertOrder<TEntity>(
        this IOrder<TEntity> order,
        string stringOrder,
        IEnumerable<OrderProperty<TEntity>> orderProperties)
    {
        var split = stringOrder.Trim().Split(Order<object>.Separator);

        order.Type
            .Should()
            .Be(split[1] == Order<object>.Descending ? OrderType.Descending : OrderType.Ascending);

        return orderProperties
            .Single(op => op.Identifier == split[0])
            .PropertySelector == order.PropertySelector;
    }
}