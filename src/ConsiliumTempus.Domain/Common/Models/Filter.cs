using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Interfaces;

namespace ConsiliumTempus.Domain.Common.Models;

public abstract class Filter<T, TEntity>(
    T value,
    Expression<Func<TEntity, T>> propertySelector,
    Func<T, bool>? canBeFiltered = null
) : IFilter<TEntity>
    where TEntity : notnull
{
    public T Value { get; } = value;
    
    public Expression<Func<TEntity, bool>> Predicate => GetPredicate();

    public bool CanBeFiltered { get; } = canBeFiltered?.Invoke(value) ?? value is not null;

    protected virtual Expression GetExpressionBody(Expression propertyExpression, Expression valueExpression)
    {
        return Expression.Equal(propertyExpression, valueExpression);
    }

    private Expression<Func<TEntity, bool>> GetPredicate()
    {
        Expression<Func<T>> valueSelector = () => Value;
        
        var propertyExpression = propertySelector.Body;
        var valueExpression = Expression.Convert(valueSelector.Body, typeof(T));
        var expressionBody = GetExpressionBody(propertyExpression, valueExpression);
        
        return Expression.Lambda<Func<TEntity, bool>>(expressionBody, propertySelector.Parameters);
    }
}