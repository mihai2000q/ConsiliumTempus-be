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
    
    public static IRuleBuilderOptions<T, string?> HasOrdersFormat<T, TEntity>(
        this IRuleBuilder<T, string?> ruleBuilder,
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        return ruleBuilder
            .Must(orders => orders.ForAll(PropertyOrderTypeSeparatorValidation))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property1}.{OrderType1}," +
                         "{Property2}.{OrderType2},...")
            .Must(orders => orders.ForAll(OrderTypeValidation))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         "where 'OrderType' must be either 'desc' or 'asc'")
            .Must(orders => orders.ForAll(SnakeCaseValidation))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         "where 'Property' must be in snake_case")
            .Must(orders => orders.ForAll(order => order.PropertyValidation(orderProperties)))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         $"where 'Property' must be a property of the entity: {typeof(TEntity).Name} " +
                         $"(available properties: {string.Join(", ", orderProperties.Select(x => x.Identifier))})")
            .Must(OrdersRepetitionValidation)
            .WithMessage("The elements of '{PropertyName}' must not repeat themselves");
    }

    private static bool ForAll(
        this string? orders,
        Func<string, bool> validator)
    {
        return orders is null || 
               orders.Split(Order<object>.ListSeparator)
                   .All(o => validator(o.Trim()));
    }

    private static bool PropertyOrderTypeSeparatorValidation(string order)
    {
        return order.Split(Order<object>.Separator).Length == 2;
    }

    private static bool OrderTypeValidation(this string order)
    {
        var propertyOrderType = order.Split(Order<object>.Separator);
        if (propertyOrderType.Length != 2) return true;
                
        var orderType = propertyOrderType[1];
        return orderType is Order<object>.Descending or Order<object>.Ascending;
    }

    private static bool SnakeCaseValidation(this string order)
    {
        var propertyOrderType = order.Split(Order<object>.Separator);
        if (propertyOrderType.Length != 2) return true;
                
        var property = propertyOrderType[0];
        return !property.Any(char.IsUpper);
    }

    private static bool PropertyValidation<TEntity>(
        this string order, 
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        var propertyOrderType = order.Split(Order<object>.Separator);
        if (propertyOrderType.Length != 2) return true;
                
        var property = propertyOrderType[0];
        return property.Any(char.IsUpper) || 
               orderProperties.Any(op => op.Identifier == property);
    }

    private static bool OrdersRepetitionValidation(this string? orders)
    {
        if (orders is null) return true;

        var propertyIdentifiers = new List<string>();
        foreach (var order in orders.Split(Order<object>.ListSeparator))
        {
            var propertyOrderType = order.Trim().Split(Order<object>.Separator);
            if (propertyOrderType.Length != 2) return true;
            propertyIdentifiers.Add(propertyOrderType[0]);
        }
        
        return propertyIdentifiers.SequenceEqual(propertyIdentifiers.Distinct());
    }
}