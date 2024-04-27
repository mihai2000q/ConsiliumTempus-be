using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Domain.Common.Interfaces;

public interface IOrder<TEntity>
{
    public Expression<Func<TEntity, object?>> PropertySelector { get; }
    public OrderType Type { get; }
}