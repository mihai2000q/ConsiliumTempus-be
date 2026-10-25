using System.Linq.Expressions;
using ConsiliumTempus.Common.UnitTests.ProjectTask.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.CustomFieldSetup;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Infrastructure.Extensions;
using ConsiliumTempus.Infrastructure.UnitTests.TestData.Extensions;

namespace ConsiliumTempus.Infrastructure.UnitTests.Extensions;

public class QueryableExtensionsTest
{
    #region Setup

    private sealed record Item(int Group, int Value);

    private sealed class TestFilter(Expression<Func<int, bool>> predicate) : IFilter<int>
    {
        public Expression<Func<int, bool>> Predicate { get; } = predicate;
    }

    private sealed class TestOrder(Expression<Func<Item, object?>> propertySelector, OrderType type) : IOrder<Item>
    {
        public Expression<Func<Item, object?>> PropertySelector { get; } = propertySelector;
        public OrderType Type { get; } = type;
    }

    private static IQueryable<int> Numbers(int count = 10) => Enumerable.Range(1, count).AsQueryable();

    private static IQueryable<Item> Items() => new[]
    {
        new Item(2, 1), new Item(1, 3), new Item(2, 2), new Item(1, 1), new Item(2, 3), new Item(1, 2)
    }.AsQueryable();

    #endregion

    [Theory]
    [ClassData(typeof(QueryableExtensionsData.GetCustomFieldSetups))]
    public void OfCustomFieldType_WhenSetupIsOfAType_ShouldReturnOnlyTheFieldsOfThatType(
        CustomFieldSetupAggregate setup,
        Type expectedFieldType)
    {
        // Arrange
        var customFields = new List<CustomField>
        {
            CustomFieldFactory.CreateDate(),
            CustomFieldFactory.CreateDateTime(),
            CustomFieldFactory.CreateDuration(),
            CustomFieldFactory.CreateMultiSelect(),
            CustomFieldFactory.CreateNumber(),
            CustomFieldFactory.CreatePeople(),
            CustomFieldFactory.CreateSingleSelect(),
            CustomFieldFactory.CreateText(),
            CustomFieldFactory.CreateTime()
        };

        // Act
        var outcome = customFields.AsQueryable().OfCustomFieldType(setup).ToList();

        // Assert
        outcome.Should().ContainSingle().Which.Should().BeOfType(expectedFieldType);
    }

    [Fact]
    public void OfCustomFieldType_WhenSetupTypeIsNotSupported_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var customFields = new List<CustomField> { CustomFieldFactory.CreateDate() }.AsQueryable();

        // Act
        var outcome = () => customFields.OfCustomFieldType(null!);

        // Assert
        outcome.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ApplyFilters_WhenThereAreNoFilters_ShouldReturnAllTheItems()
    {
        // Arrange - nothing

        // Act
        var outcome = Numbers().ApplyFilters([]).ToList();

        // Assert
        outcome.Should().Equal(Numbers().ToList());
    }

    [Fact]
    public void ApplyFilters_WhenThereIsOneFilter_ShouldReturnTheItemsThatMatchIt()
    {
        // Arrange
        var filters = new[] { new TestFilter(n => n > 7) };

        // Act
        var outcome = Numbers().ApplyFilters(filters).ToList();

        // Assert
        outcome.Should().Equal(8, 9, 10);
    }

    [Fact]
    public void ApplyFilters_WhenThereAreMultipleFilters_ShouldReturnTheItemsThatMatchAllOfThem()
    {
        // Arrange
        var filters = new[]
        {
            new TestFilter(n => n > 3),
            new TestFilter(n => n < 8),
            new TestFilter(n => n % 2 == 0)
        };

        // Act
        var outcome = Numbers().ApplyFilters(filters).ToList();

        // Assert
        outcome.Should().Equal(4, 6);
    }

    [Fact]
    public void ApplyOrders_WhenThereAreNoOrders_ShouldKeepTheOriginalOrder()
    {
        // Arrange - nothing

        // Act
        var outcome = Items().ApplyOrders([]).ToList();

        // Assert
        outcome.Should().Equal(Items().ToList());
    }

    [Theory]
    [InlineData(OrderType.Ascending, new[] { 1, 1, 1, 2, 2, 2 })]
    [InlineData(OrderType.Descending, new[] { 2, 2, 2, 1, 1, 1 })]
    public void ApplyOrders_WhenThereIsOneOrder_ShouldOrderByItInTheGivenDirection(
        OrderType type,
        int[] expectedGroups)
    {
        // Arrange
        var orders = new[] { new TestOrder(i => i.Group, type) };

        // Act
        var outcome = Items().ApplyOrders(orders).ToList();

        // Assert
        outcome.Select(i => i.Group).Should().Equal(expectedGroups);
    }

    [Theory]
    [InlineData(OrderType.Ascending, OrderType.Ascending, new[] { 1, 2, 3, 1, 2, 3 })]
    [InlineData(OrderType.Ascending, OrderType.Descending, new[] { 3, 2, 1, 3, 2, 1 })]
    [InlineData(OrderType.Descending, OrderType.Ascending, new[] { 1, 2, 3, 1, 2, 3 })]
    [InlineData(OrderType.Descending, OrderType.Descending, new[] { 3, 2, 1, 3, 2, 1 })]
    public void ApplyOrders_WhenThereAreMultipleOrders_ShouldApplyTheFirstOneAndThenBreakTheTiesWithTheOthers(
        OrderType groupOrder,
        OrderType valueOrder,
        int[] expectedValues)
    {
        // Arrange
        var orders = new[]
        {
            new TestOrder(i => i.Group, groupOrder),
            new TestOrder(i => i.Value, valueOrder)
        };

        // Act
        var outcome = Items().ApplyOrders(orders).ToList();

        // Assert
        outcome.Select(i => i.Value).Should().Equal(expectedValues);
        outcome.Select(i => i.Group).Should().BeInAscendingOrder(
            groupOrder == OrderType.Ascending ? Comparer<int>.Default : Comparer<int>.Create((a, b) => b.CompareTo(a)));
    }

    [Fact]
    public void Paginate_WhenThereIsNoPaginationInfo_ShouldReturnAllTheItems()
    {
        // Arrange - nothing

        // Act
        var outcome = Numbers().Paginate(null).ToList();

        // Assert
        outcome.Should().Equal(Numbers().ToList());
    }

    [Theory]
    [InlineData(3, 1, new[] { 1, 2, 3 })]
    [InlineData(3, 2, new[] { 4, 5, 6 })]
    [InlineData(3, 4, new[] { 10 })]
    [InlineData(3, 5, new int[0])]
    [InlineData(20, 1, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    public void Paginate_WhenThereIsPaginationInfo_ShouldReturnTheItemsOfThePage(
        int pageSize,
        int currentPage,
        int[] expected)
    {
        // Arrange
        var paginationInfo = PaginationInfo.Create(pageSize, currentPage);

        // Act
        var outcome = Numbers().Paginate(paginationInfo).ToList();

        // Assert
        outcome.Should().Equal(expected);
    }

    [Theory]
    [InlineData(true, new[] { 6, 7, 8, 9, 10 })]
    [InlineData(false, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    public void WhereIf_ShouldApplyThePredicateOnlyWhenTheConditionIsTrue(bool condition, int[] expected)
    {
        // Arrange - parameterized

        // Act
        var outcome = Numbers().WhereIf(condition, n => n > 5).ToList();

        // Assert
        outcome.Should().Equal(expected);
    }

    [Theory]
    [InlineData(true, new[] { 1, 1, 1, 2, 2, 2 })]
    [InlineData(false, new[] { 2, 1, 2, 1, 2, 1 })]
    public void OrderByIf_ShouldOrderOnlyWhenTheConditionIsTrue(bool condition, int[] expectedGroups)
    {
        // Arrange - parameterized

        // Act
        var outcome = Items().OrderByIf(condition, i => i.Group).ToList();

        // Assert
        outcome.Select(i => i.Group).Should().Equal(expectedGroups);
    }
}
