using System.Linq.Expressions;
using System.Reflection;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Common.Models;

public static class Filter
{
    public const char Separator = ' ';

    public static readonly Dictionary<string, FilterOperator> OperatorToFilterOperator = new()
    {
        { "eq", FilterOperator.Equal },
        { "neq", FilterOperator.NotEqual },
        { "gt", FilterOperator.GreaterThan },
        { "gte", FilterOperator.GreaterThanOrEqual },
        { "lt", FilterOperator.LessThan },
        { "lte", FilterOperator.LessThanOrEqual },
        { "ct", FilterOperator.Contains },
        { "sw", FilterOperator.StartsWith }
    };

    internal static readonly MethodInfo ContainsMethod = typeof(string)
        .GetMethods()
        .Where(m => m.Name == nameof(string.Contains))
        .Where(m => m.GetParameters().Length == 1)
        .Single(m => m.GetParameters().First().ParameterType == string.Empty.GetType());

    internal static readonly MethodInfo StartsWithMethod = typeof(string)
        .GetMethods()
        .Where(m => m.Name == nameof(string.StartsWith))
        .Where(m => m.GetParameters().Length == 1)
        .Single(m => m.GetParameters().First().ParameterType == string.Empty.GetType());
}

public class Filter<TEntity>(Expression<Func<TEntity, bool>> predicate) : IFilter<TEntity>
    where TEntity : notnull
{
    public Expression<Func<TEntity, bool>> Predicate { get; } = predicate;

    protected static IReadOnlyList<IFilter<TEntity>> Parse(
        string[]? filters,
        IReadOnlyList<FilterProperty<TEntity>> filterProperties)
    {
        if (filters is null) return [];

        return filters
            .Select(s => ParseFilter(s, filterProperties))
            .ToList();
    }

    private static Filter<TEntity> ParseFilter(
        string filter,
        IReadOnlyList<FilterProperty<TEntity>> filterProperties)
    {
        var (identifier, @operator, value) = SplitFilter(filter.Trim());

        var filterProperty = filterProperties.Single(fp => fp.Identifier == identifier);

        var propertyExpression = filterProperty.PropertySelector.Body;
        var valueExpression = GetValueExpression(filterProperty.PropertySelector.ReturnType, value.TrimStart());
        var expressionBody = GetExpressionBody(
            propertyExpression,
            valueExpression,
            Filter.OperatorToFilterOperator[@operator]);

        var predicate = Expression.Lambda<Func<TEntity, bool>>(
            expressionBody,
            filterProperty.PropertySelector.Parameters);

        return new Filter<TEntity>(predicate);
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

    private static ConstantExpression GetValueExpression(Type type, string value)
    {
        return type switch
        {
            not null when type == typeof(bool) => Expression.Constant(bool.Parse(value)),
            not null when type == typeof(DateTime) => Expression.Constant(DateTime.Parse(value)),
            not null when type == typeof(decimal) => Expression.Constant(decimal.Parse(value)),
            not null when type == typeof(int) => Expression.Constant(int.Parse(value)),
            _ => Expression.Constant(value)
        };
    }

    private static Expression GetExpressionBody(
        Expression propertyExpression,
        Expression valueExpression,
        FilterOperator filterOperator)
    {
        return filterOperator switch
        {
            FilterOperator.Equal => Expression.Equal(propertyExpression, valueExpression),
            FilterOperator.NotEqual => Expression.NotEqual(propertyExpression, valueExpression),
            FilterOperator.GreaterThan => Expression.GreaterThan(propertyExpression, valueExpression),
            FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(propertyExpression, valueExpression),
            FilterOperator.LessThan => Expression.LessThan(propertyExpression, valueExpression),
            FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(propertyExpression, valueExpression),
            FilterOperator.Contains => Expression.Call(propertyExpression, Filter.ContainsMethod, valueExpression),
            FilterOperator.StartsWith => Expression.Call(propertyExpression, Filter.StartsWithMethod, valueExpression),
            _ => throw new ArgumentOutOfRangeException(nameof(filterOperator), filterOperator, null)
        };
    }
}