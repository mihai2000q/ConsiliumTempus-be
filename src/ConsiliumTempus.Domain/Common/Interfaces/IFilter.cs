using System.Linq.Expressions;

namespace ConsiliumTempus.Domain.Common.Interfaces;

public interface IFilter<TEntity>
{
    Expression<Func<TEntity, bool>> Predicate { get; }
}