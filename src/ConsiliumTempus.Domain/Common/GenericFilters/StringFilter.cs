using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.GenericFilters;

[SuppressMessage("ReSharper", "StaticMemberInGenericType")]
public abstract class StringFilter<TEntity>(
    string? value,
    Expression<Func<TEntity, string?>> propertySelector) 
    : Filter<string?, TEntity>(value, propertySelector, StringCondition)
    where TEntity : notnull
{
    private static readonly MethodInfo ContainsMethod = typeof(string)
        .GetMethods()
        .Where(m => m.Name == nameof(string.Contains))
        .Where(m => m.GetParameters().Length == 1)
        .Single(m => m.GetParameters().First().ParameterType == "".GetType());
    private static readonly Func<string?, bool> StringCondition = v => !string.IsNullOrWhiteSpace(v);
    
    protected override Expression GetExpressionBody(Expression propertyExpression, Expression valueExpression)
    {
        return Expression.Call(propertyExpression, ContainsMethod, valueExpression);
    }
}