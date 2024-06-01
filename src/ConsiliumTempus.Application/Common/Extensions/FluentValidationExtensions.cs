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

    public static IRuleBuilderOptions<T, string[]?> HasOrderByFormat<T, TEntity>(
        this IRuleBuilder<T, string[]?> ruleBuilder,
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        return ruleBuilder
            .Must(orders => orders.ForAll(OrderByValidation.SeparatorValidation))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType} " +
                         "(separated by a '.')")
            .Must(orders => orders.ForAll(OrderByValidation.OrderTypeValidation))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         "where 'OrderType' must be either 'desc' or 'asc'")
            .Must(orders => orders.ForAll(OrderByValidation.SnakeCaseValidation))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         "where 'Property' must be in snake_case")
            .Must(orders => orders.ForAll(order => OrderByValidation.PropertyValidation(order, orderProperties)))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         $"where 'Property' must be a property of the entity: {typeof(TEntity).Name} " +
                         "(available properties: " +
                         $"{string.Join(", ", orderProperties.Select(x => x.Identifier))})")
            .Must(OrderByValidation.OrdersRepetitionValidation)
            .WithMessage("The elements of '{PropertyName}' must not repeat themselves");
    }

    private static bool ForAll(
        this string[]? orders,
        Func<string, bool> validator)
    {
        return orders is null || orders.All(validator);
    }

    private static bool PropertyOrderTypeSeparatorValidation(string order)
    {
        return order.Split(Order<object>.Separator).Length == 2;
    }

    private static class OrderByValidation
    {
        internal static bool SeparatorValidation(string order)
        {
            return order.Split(Order.Separator).Length == 2;
        }

        internal static bool OrderTypeValidation(string order)
        {
            var propertyOrderType = order.Split(Order.Separator);
            if (propertyOrderType.Length != 2) return true;

            var orderType = propertyOrderType[1];
            return orderType is Order.Descending or Order.Ascending;
        }

        internal static bool SnakeCaseValidation(string order)
        {
            var propertyOrderType = order.Split(Order.Separator);
            if (propertyOrderType.Length != 2) return true;

            var property = propertyOrderType[0].Trim();
            return !property.Any(char.IsUpper);
        }

        internal static bool PropertyValidation<TEntity>(
            string order,
            IReadOnlyList<OrderProperty<TEntity>> orderProperties)
        {
            var propertyOrderType = order.Split(Order.Separator);
            if (propertyOrderType.Length != 2) return true;

            var property = propertyOrderType[0].Trim();
            return property.Any(char.IsUpper) ||
                   orderProperties.Any(op => op.Identifier == property);
        }

        internal static bool OrdersRepetitionValidation(string[]? orders)
        {
            if (orders is null) return true;

            var propertyIdentifiers = new List<string>();
            foreach (var order in orders)
            {
                var propertyOrderType = order.Trim().Split(Order.Separator);
                if (propertyOrderType.Length != 2) return true;
                propertyIdentifiers.Add(propertyOrderType[0]);
            }

            return propertyIdentifiers.SequenceEqual(propertyIdentifiers.Distinct());
        }
    }

    private static bool OrdersRepetitionValidation(this string[]? orders)
    {
        if (orders is null) return true;

        var propertyIdentifiers = new List<string>();
        foreach (var order in orders)
        {
            var propertyOrderType = order.Trim().Split(Order<object>.Separator);
            if (propertyOrderType.Length != 2) return true;
            propertyIdentifiers.Add(propertyOrderType[0]);
        }
        
        return propertyIdentifiers.SequenceEqual(propertyIdentifiers.Distinct());
    }
}