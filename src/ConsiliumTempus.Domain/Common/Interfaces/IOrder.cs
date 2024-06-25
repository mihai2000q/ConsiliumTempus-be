using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Domain.Common.Interfaces;

public interface IOrder<TEntity>
{
    Expression<Func<TEntity, object?>> PropertySelector { get; }
    OrderType Type { get; }
}