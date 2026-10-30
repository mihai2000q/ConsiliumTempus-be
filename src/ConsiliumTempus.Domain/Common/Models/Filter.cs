using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Project.Entities;

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

    public static (string PropertyIdentifier, string Operator, string Value)? Split(string filter)
    {
        filter = filter.Trim();
        var result = new List<string>();

        var current = "";
        for (var i = 0; i < filter.Length; i++)
            if (filter[i] == Separator)
            {
                result.Add(current);
                current = "";
                if (result.Count != 2 || i == filter.Length - 1) continue;
                result.Add(filter[(i + 1)..].TrimStart());
                break;
            }
            else
                current += filter[i];

        if (result.Count != 3) return null;

        return (result[0], result[1], result[2]);
    }

    public static bool TryParseValue(Type type, string value, [NotNullWhen(true)] out object? result)
    {
        result = type switch
        {
            _ when type == typeof(bool) => bool.TryParse(value, out var b) ? b : null,
            _ when type == typeof(DateTime) => DateTime.TryParse(value, out var dt) ? dt : null,
            _ when type == typeof(decimal) => decimal.TryParse(value, out var d) ? d : null,
            _ when type == typeof(int) => int.TryParse(value, out var i) ? i : null,
            _ when type.IsEnum => Enum.TryParse(type, value, true, out var e) ? e : null,
            _ => value
        };
        return result is not null;
    }
}

public class Filter<TEntity>(Expression<Func<TEntity, bool>> predicate) : IFilter<TEntity>
    where TEntity : notnull
{
    public Expression<Func<TEntity, bool>> Predicate { get; } = predicate;

    protected static IReadOnlyList<IFilter<TEntity>> Parse(
        List<string> filters,
        IReadOnlyList<FilterProperty<TEntity>> filterProperties)
    {
        return filters
            .Select(s => ParseFilter(s, filterProperties))
            .ToList();
    }

    private static Filter<TEntity> ParseFilter(
        string filter,
        IReadOnlyList<FilterProperty<TEntity>> filterProperties)
    {
        var (identifier, @operator, value) = Filter.Split(filter)!.Value;

        var filterProperty = filterProperties.Single(fp => fp.Identifier == identifier);

        var propertyExpression = filterProperty.PropertySelector.Body;
        var valueExpression = GetValueExpression(filterProperty.PropertySelector.ReturnType, value);
        var expressionBody = GetExpressionBody(
            identifier,
            propertyExpression,
            valueExpression,
            Filter.OperatorToFilterOperator[@operator]);

        var predicate = Expression.Lambda<Func<TEntity, bool>>(
            expressionBody,
            filterProperty.PropertySelector.Parameters);

        return new Filter<TEntity>(predicate);
    }

    private static ConstantExpression GetValueExpression(Type type, string value)
    {
        if (!Filter.TryParseValue(type, value, out var parsedValue))
            throw new FormatException($"The value '{value}' cannot be parsed to {type.Name}");

        return Expression.Constant(parsedValue);
    }

    private static Expression GetExpressionBody(
        string identifier,
        Expression propertyExpression,
        ConstantExpression valueExpression,
        FilterOperator filterOperator)
    {
        if (propertyExpression.Type == typeof(Enumerable))
        {
            return GetEnumerableExpressionCall<ProjectStatus>(
                identifier.ToUpper(), // TODO: From Snake Case to Pascal Case
                nameof(Enumerable.Any), // TODO: More Options (add List operator)
                valueExpression,
                propertyExpression,
                filterOperator);
        }

        return GetExpressionByFilterOperator(
            propertyExpression,
            valueExpression,
            filterOperator);
    }

    private static MethodCallExpression GetEnumerableExpressionCall<TCollectionEntity>(
        string entityName,
        string method,
        ConstantExpression valueExpression,
        Expression propertyExpression,
        FilterOperator filterOperator)
    {
        var param = Expression.Parameter(typeof(TCollectionEntity));
        var property = Expression.Property(param, entityName);
        var equality = GetExpressionByFilterOperator(property, valueExpression, filterOperator);

        return Expression.Call(
            typeof(Enumerable),
            method,
            [typeof(TCollectionEntity)],
            propertyExpression,
            Expression.Lambda<Func<TCollectionEntity, bool>>(equality, param)
        );
    }

    private static Expression GetExpressionByFilterOperator(
        Expression propertyExpression,
        ConstantExpression valueExpression,
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