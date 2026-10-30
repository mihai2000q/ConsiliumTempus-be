using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Extensions;

namespace ConsiliumTempus.Domain.Common.Models;

public abstract class FilterProperty<TEntity>(
    string identifier,
    Expression<Func<TEntity, object>> selector)
{
    public string Identifier { get; } = identifier.UpperCamelCaseToSnakeCase();
    public LambdaExpression PropertySelector { get; } = StripConvert(selector);

    private static LambdaExpression StripConvert<T>(Expression<Func<T, object>> source)
    {
        var result = source.Body;
        // use a loop in case there are nested Convert expressions
        while (result.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked
               && result.Type == typeof(object))
        {
            result = ((UnaryExpression)result).Operand;
        }

        return Expression.Lambda(result, source.Parameters);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() &&
               Equals((FilterProperty<TEntity>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Identifier, PropertySelector);
    }

    private bool Equals(FilterProperty<TEntity> other)
    {
        return Identifier == other.Identifier && PropertySelector.Equals(other.PropertySelector);
    }
}