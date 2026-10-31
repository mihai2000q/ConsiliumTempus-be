using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Extensions;

namespace ConsiliumTempus.Domain.Common.Models;

public sealed record OrderProperty<TEntity>(
    string Identifier,
    params Expression<Func<TEntity, object?>>[] PropertySelectors)
{
    public string Identifier { get; } = Identifier.UpperCamelCaseToSnakeCase();

    // Expression trees only have reference equality, so the Identifier (unique per entity) defines the property
    public bool Equals(OrderProperty<TEntity>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityContract == other.EqualityContract && Identifier == other.Identifier;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EqualityContract, Identifier);
    }
}
