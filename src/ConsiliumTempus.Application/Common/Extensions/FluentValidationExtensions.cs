using ConsiliumTempus.Domain.Common.Models;
using FluentValidation;

namespace ConsiliumTempus.Application.Common.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> IsPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(p => p.Length > 8)
            .WithMessage("'{PropertyName}' must be longer than 8 characters")
            .Must(p => p.ContainsUppercase())
            .WithMessage("'{PropertyName}' must contain an uppercase letter")
            .Must(p => p.ContainsLowercase())
            .WithMessage("'{PropertyName}' must contain a lowercase letter")
            .Must(p => p.ContainsNumber())
            .WithMessage("'{PropertyName}' must contain at least a number");
    }

    public static IRuleBuilderOptions<T, string> IsEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(e => Regex.RegexStore.EmailRegex().IsMatch(e))
            .WithMessage("'{PropertyName}' must be valid email");
    }

    public static IRuleBuilderOptions<T, DateOnly?> IsPastDate<T>(this IRuleBuilder<T, DateOnly?> ruleBuilder)
    {
        return ruleBuilder.LessThan(DateOnly.FromDateTime(DateTime.UtcNow));
    }
    
    public static IRuleBuilderOptions<T, string?> HasOrderFormat<T, TEntity>(
        this IRuleBuilder<T, string?> ruleBuilder,
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        return ruleBuilder
            .Must(o => o is null || o.Split(Order<object>.Separator).Length == 2)
            .WithMessage("'{PropertyName}' must have the following format: {Property}.{OrderType}")
            .Must(o =>
            {
                if (o is null) return true;
                var split = o.Split(Order<object>.Separator);
                if (split.Length != 2) return true;
                
                var orderType = split[1];
                return orderType is Order<object>.Descending or Order<object>.Ascending;
            })
            .WithMessage("'{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         "where 'OrderType' must be either 'desc' or 'asc'")
            .Must(o =>
            {
                if (o is null) return true;
                var split = o.Split(Order<object>.Separator);
                if (split.Length != 2) return true;
                
                var property = split[0];
                return !property.Where(char.IsUpper).Any();
            })
            .WithMessage("'{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         "where 'Property' must be in snake_case")
            .Must(o =>
            {
                if (o is null) return true;
                var split = o.Split(Order<object>.Separator);
                if (split.Length != 2) return true;
                
                var property = split[0];
                return property.Where(char.IsUpper).Any() || 
                       orderProperties.Any(op => op.Identifier == property);
            })
            .WithMessage("'{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         $"where 'Property' must be a property of the entity: {typeof(TEntity).Name}");
    }
}