using System.ComponentModel;
using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;
using FluentAssertions.Extensions;

namespace ConsiliumTempus.Application.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static readonly TimeSpan TimeSpanPrecision = 15.Seconds();

    internal static void ValidateError<T>(this ErrorOr<T> error, Error expectedError)
    {
        error.IsError.Should().BeTrue();
        error.Errors.Should().HaveCount(1);
        error.FirstError.Should().Be(expectedError);
    }

    internal static void ShouldBeOrdered(this IReadOnlyList<ProjectTaskAggregate> tasks)
    {
        var customOrderPosition = 0;
        tasks.OrderBy(t => t.CustomOrderPosition)
            .Should().AllSatisfyOrEmpty(t =>
                t.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    internal static void ShouldBeOrdered(this IReadOnlyList<ProjectStage> stages)
    {
        var customOrderPosition = 0;
        stages.OrderBy(s => s.CustomOrderPosition)
            .Should().AllSatisfyOrEmpty(s => s.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    internal static void ShouldBeOrdered(this IReadOnlyList<SingleSelectOption> options)
    {
        var customOrderPosition = 0;
        options.OrderBy(s => s.CustomOrderPosition)
            .Should().AllSatisfyOrEmpty(o => o.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    internal static void ShouldBeOrdered(this IReadOnlyList<MultiSelectOption> options)
    {
        var customOrderPosition = 0;
        options.OrderBy(s => s.CustomOrderPosition)
            .Should().AllSatisfyOrEmpty(o => o.CustomOrderPosition.Value.Should().Be(customOrderPosition++));
    }

    extension(Audit audit)
    {
        internal void ShouldBeCreated(UserAggregate createdBy)
        {
            audit.CreatedBy.Should().Be(createdBy);
            audit.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
            audit.UpdatedBy.Should().Be(createdBy);
            audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }

        internal void ShouldBeUpdated(UserAggregate updatedBy)
        {
            audit.UpdatedBy.Should().Be(updatedBy);
            audit.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpanPrecision);
        }
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
        List<string> orderBy,
        IEnumerable<OrderProperty<TEntity>> orderProperties)
    {
        orderBy.Should().HaveSameCount(orders);
        return orders
            .Zip(orderBy)
            .All(x => x.First.AssertOrder(x.Second, orderProperties));
    }

    internal static bool AssertFilters<TEntity>(
        this IReadOnlyList<IFilter<TEntity>> filters,
        List<string> search,
        IEnumerable<FilterProperty<TEntity>> filterProperties)
    {
        filters.Should().HaveSameCount(search);
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
            .PropertySelectors
            .Any(propertySelector => propertySelector == order.PropertySelector);
    }

    private static bool AssertFilter<TEntity>(
        this IFilter<TEntity> filter,
        string stringFilter,
        IEnumerable<FilterProperty<TEntity>> filterProperties)
    {
        var (identifier, @operator, value) = SplitFilter(stringFilter.Trim());
        var selector = filterProperties.Single(fp => fp.Identifier == identifier).PropertySelector;

        filter.Predicate.Parameters.Should().Equal(selector.Parameters);

        var expectedFilterOperator = Filter.OperatorToFilterOperator[@operator];
        var expectedValue = ParseFilterValue(selector.ReturnType, value.TrimStart());

        Expression propertyExpression;
        Expression valueExpression;
        switch (filter.Predicate.Body)
        {
            case BinaryExpression binary:
                binary.NodeType.Should().Be(ToExpressionType(expectedFilterOperator));
                propertyExpression = binary.Left;
                valueExpression = binary.Right;
                break;
            case MethodCallExpression { Object: not null } call:
                call.Method.DeclaringType.Should().Be<string>();
                call.Method.Name.Should().Be(expectedFilterOperator == FilterOperator.Contains
                    ? nameof(string.Contains)
                    : nameof(string.StartsWith));
                call.Arguments.Should().ContainSingle();
                propertyExpression = call.Object;
                valueExpression = call.Arguments[0];
                break;
            default:
                throw new InvalidOperationException(
                    $"Unexpected filter predicate: {filter.Predicate.Body}");
        }

        propertyExpression.Should().BeSameAs(selector.Body);
        valueExpression.Should().BeOfType<ConstantExpression>()
            .Which.Value.Should().Be(expectedValue);

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

    private static ExpressionType ToExpressionType(FilterOperator filterOperator)
    {
        return filterOperator switch
        {
            FilterOperator.Equal => ExpressionType.Equal,
            FilterOperator.NotEqual => ExpressionType.NotEqual,
            FilterOperator.GreaterThan => ExpressionType.GreaterThan,
            FilterOperator.GreaterThanOrEqual => ExpressionType.GreaterThanOrEqual,
            FilterOperator.LessThan => ExpressionType.LessThan,
            FilterOperator.LessThanOrEqual => ExpressionType.LessThanOrEqual,
            _ => throw new ArgumentOutOfRangeException(nameof(filterOperator), filterOperator, null)
        };
    }

    private static object? ParseFilterValue(Type type, string value)
    {
        if (type == typeof(DateTime)) return DateTime.Parse(value);
        return TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
    }
}