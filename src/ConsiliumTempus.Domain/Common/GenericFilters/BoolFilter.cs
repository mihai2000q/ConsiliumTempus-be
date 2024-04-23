using System.Linq.Expressions;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.GenericFilters;

public abstract class BoolFilter<TEntity>(
    bool? value,
    Expression<Func<TEntity, bool?>> propertySelector) 
    : Filter<bool?, TEntity>(value, propertySelector)
    where TEntity : notnull;