using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Extensions;

namespace ConsiliumTempus.Domain.Common.Models;

public sealed class FilterProperty<TEntity>(
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

    // Expression trees only have reference equality, so the Identifier (unique per entity) defines the property
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() &&
               Identifier == ((FilterProperty<TEntity>)obj).Identifier;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Identifier);
    }
}
