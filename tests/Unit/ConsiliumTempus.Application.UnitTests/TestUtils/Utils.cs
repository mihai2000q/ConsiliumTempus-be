using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.User;
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

    internal static void ShouldBeOrdered(this IReadOnlyList<ProjectStage> stages)
    {
        var customOrderPosition = 0;
        stages.OrderBy(s => s.CustomOrderPosition.Value)
            .Should().AllSatisfy(s => s.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    internal static void ShouldBeCreated(this Audit audit, UserAggregate createdBy)
    {
        audit.CreatedBy.Should().Be(createdBy);
        audit.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        audit.UpdatedBy.Should().Be(createdBy);
        audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
    }
    
    internal static void ShouldBeUpdated(this Audit audit, UserAggregate updatedBy)
    {
        audit.UpdatedBy.Should().Be(updatedBy);
        audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
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
        string[]? orderBy,
        IEnumerable<OrderProperty<TEntity>> orderProperties)
    {
        if (orderBy is null) return orders.Count == 0;
        return orders
            .Zip(orderBy)
            .All(x => x.First.AssertOrder(x.Second, orderProperties));
    }
    
    internal static bool AssertFilters<TEntity>(
        this IReadOnlyList<IFilter<TEntity>> filters,
        string[]? search,
        IEnumerable<FilterProperty<TEntity>> filterProperties)
    {
        if (search is null) return filters.Count == 0;
        return filters
            .Zip(search)
            .All(x => x.First.AssertFilter(x.Second, filterProperties));
    }
    
    private static bool AssertOrder<TEntity>(
        this IOrder<TEntity> order,
        string stringOrder,
        IEnumerable<OrderProperty<TEntity>> orderProperties)
    {
        var split = stringOrder.Trim().Split(Order.Separator);

        order.Type
            .Should()
            .Be(split[1] == Order.Descending ? OrderType.Descending : OrderType.Ascending);

        return orderProperties
            .Single(op => op.Identifier == split[0])
            .PropertySelector == order.PropertySelector;
    }

    private static bool AssertFilter<TEntity>(
        this IFilter<TEntity> filter,
        string stringFilter,
        IEnumerable<FilterProperty<TEntity>> filterProperties)
    {
        var (propertyIdentifier, @operator, value) = SplitFilter(stringFilter);

        return true;
    }
    
    private static (string, string, string) SplitFilter(string filter)
    {
        var result = new List<string>();

        var current = "";
        for (var i = 0; i < filter.Length; i++)
            if (filter[i] == Filter.Separator)
            {
                result.Add(current);
                current = "";
                if (result.Count != 2) continue;
                result.Add(filter[(i + 1)..]);
                break;
            }
            else
                current += filter[i];

        return (result[0], result[1], result[2]);
    }
}