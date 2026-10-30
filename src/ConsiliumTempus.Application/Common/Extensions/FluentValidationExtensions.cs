using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Models;
using FluentValidation;

namespace ConsiliumTempus.Application.Common.Extensions;

internal static class FluentValidationExtensions
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
        return ruleBuilder.Matches(Regex.RegexStore.EmailRegex())
            .WithMessage("'{PropertyName}' must be valid email");
    }

    public static IRuleBuilderOptions<T, DateOnly?> IsPastDate<T>(this IRuleBuilder<T, DateOnly?> ruleBuilder)
    {
        return ruleBuilder.LessThan(DateOnly.FromDateTime(DateTime.UtcNow));
    }

    public static IRuleBuilderOptions<T, string> IsWorkspaceRole<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(wr => WorkspaceRole.FromName(wr.Capitalize()) is not null)
            .WithMessage("'{PropertyName}' must be valid workspace role");
    }

    public static IRuleBuilderOptions<T, string?> IsCurrencyCode<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Matches("^[A-Z]{3}$")
            .WithMessage("'{PropertyName}' must be valid Currency Code");
    }

    public static IRuleBuilderOptions<T, string> IsColor<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(Regex.RegexStore.ColorRegex())
            .WithMessage("'{PropertyName}' must be valid color");
    }

    public static IRuleBuilderOptions<T, List<string>> HasOrderByFormat<T, TEntity>(
        this IRuleBuilder<T, List<string>> ruleBuilder,
        IReadOnlyList<OrderProperty<TEntity>> orderProperties)
    {
        return ruleBuilder
            .Must(orders => orders.All(OrderByValidation.SeparatorValidation))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType} " +
                         "(separated by a '.')")
            .Must(orders => orders.All(OrderByValidation.OrderTypeValidation))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         "where 'OrderType' must be either 'desc' or 'asc'")
            .Must(orders => orders.All(OrderByValidation.SnakeCaseValidation))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         "where 'Property' must be in snake_case")
            .Must(orders => orders.All(order => OrderByValidation.PropertyValidation(order, orderProperties)))
            .WithMessage("The elements of '{PropertyName}' must have the following format: {Property}.{OrderType}, " +
                         $"where 'Property' must be a property of the entity: {typeof(TEntity).Name} " +
                         "(available properties: " +
                         $"{string.Join(", ", orderProperties.Select(x => x.Identifier))})")
            .Must(OrderByValidation.OrdersRepetitionValidation)
            .WithMessage("The elements of '{PropertyName}' must not repeat themselves");
    }

    public static IRuleBuilderOptions<T, List<string>> HasSearchFormat<T, TEntity>(
        this IRuleBuilder<T, List<string>> ruleBuilder,
        IReadOnlyList<FilterProperty<TEntity>> filterProperties)
    {
        return ruleBuilder
            .Must(search => search.All(SearchValidation.SeparatorValidation))
            .WithMessage(
                "The elements of '{PropertyName}' must have the following format: {Property} {Operator} {Value} " +
                "(separated by whitespaces)")
            .Must(search => search.All(SearchValidation.SnakeCaseValidation))
            .WithMessage(
                "The elements of '{PropertyName}' must have the following format: {Property} {Operator} {Value}, " +
                "where 'Property' must be in snake case")
            .Must(search => search.All(f => SearchValidation.PropertyValidation(f, filterProperties)))
            .WithMessage(
                "The elements of '{PropertyName}' must have the following format: {Property} {Operator} {Value}, " +
                $"where 'Property' must be a property of the entity: {typeof(TEntity).Name} " +
                "(available properties: " +
                $"{string.Join(", ", filterProperties.Select(x => x.Identifier))})")
            .Must(search => search.All(SearchValidation.OperatorValidation))
            .WithMessage(
                "The elements of '{PropertyName}' must have the following format: {Property} {Operator} {Value}, " +
                "where 'Operator' must be one of the following: " +
                $"{string.Join(", ", Filter.OperatorToFilterOperator.Keys)}")
            .Must(search => search.All(f => SearchValidation.OperatorAndValueTypeValidation(f, filterProperties)))
            .WithMessage("The '{PropertyName}' must have a supported combination of Type and Operator")
            .Must(search => search.All(f => SearchValidation.ValueParsingValidation(f, filterProperties)))
            .WithMessage(
                "The Value from the '{PropertyName}', given the type of the filter proposed, cannot be parsed");
    }

    private static class OrderByValidation
    {
        internal static bool SeparatorValidation(string order)
        {
            return Order.Split(order) is not null;
        }

        internal static bool OrderTypeValidation(string order)
        {
            var splitOrder = Order.Split(order);
            return splitOrder is null ||
                   splitOrder.Value.OrderType is Order.Descending or Order.Ascending;
        }

        internal static bool SnakeCaseValidation(string order)
        {
            var splitOrder = Order.Split(order);
            return splitOrder is null ||
                   !splitOrder.Value.PropertyIdentifier.Any(char.IsUpper);
        }

        internal static bool PropertyValidation<TEntity>(
            string order,
            IReadOnlyList<OrderProperty<TEntity>> orderProperties)
        {
            var splitOrder = Order.Split(order);
            if (splitOrder is null) return true;

            var property = splitOrder.Value.PropertyIdentifier;
            return property.Any(char.IsUpper) ||
                   orderProperties.Any(op => op.Identifier == property);
        }

        internal static bool OrdersRepetitionValidation(List<string> orders)
        {
            var propertyIdentifiers = new List<string>();
            foreach (var order in orders)
            {
                var splitOrder = Order.Split(order);
                if (splitOrder is null) return true;
                propertyIdentifiers.Add(splitOrder.Value.PropertyIdentifier);
            }

            return propertyIdentifiers.SequenceEqual(propertyIdentifiers.Distinct());
        }
    }

    private static class SearchValidation
    {
        internal static bool SeparatorValidation(string filter)
        {
            return Filter.Split(filter) is not null;
        }

        internal static bool SnakeCaseValidation(string filter)
        {
            var parsedFilter = Filter.Split(filter);
            return parsedFilter is null ||
                   !parsedFilter.Value.PropertyIdentifier.Any(char.IsUpper);
        }

        internal static bool PropertyValidation<TEntity>(
            string filter,
            IReadOnlyList<FilterProperty<TEntity>> filterProperties)
        {
            var parsedFilter = Filter.Split(filter);
            if (parsedFilter is null) return true;

            var property = filterProperties
                .SingleOrDefault(fp => fp.Identifier == parsedFilter.Value.PropertyIdentifier);
            return parsedFilter.Value.PropertyIdentifier.Any(char.IsUpper) ||
                   property is not null;
        }

        internal static bool OperatorValidation(string filter)
        {
            var parsedFilter = Filter.Split(filter);
            return parsedFilter is null ||
                   Filter.OperatorToFilterOperator.TryGetValue(parsedFilter.Value.Operator, out _);
        }

        internal static bool OperatorAndValueTypeValidation<TEntity>(
            string filter,
            IReadOnlyList<FilterProperty<TEntity>> filterProperties)
        {
            var parsedFilter = Filter.Split(filter);
            if (parsedFilter is null) return true;

            var property = filterProperties
                .SingleOrDefault(fp => fp.Identifier == parsedFilter.Value.PropertyIdentifier);

            return property is null ||
                   !Filter.OperatorToFilterOperator.TryGetValue(parsedFilter.Value.Operator, out var filterOperator) ||
                   SupportedOperators(property.PropertySelector.ReturnType).Contains(filterOperator);
        }

        internal static bool ValueParsingValidation<TEntity>(
            string filter,
            IReadOnlyList<FilterProperty<TEntity>> filterProperties)
        {
            var parsedFilter = Filter.Split(filter);
            if (parsedFilter is null) return true;

            var property = filterProperties
                .SingleOrDefault(fp => fp.Identifier == parsedFilter.Value.PropertyIdentifier);
            if (property is null || SupportedOperators(property.PropertySelector.ReturnType).Length == 0)
                return true;

            return Filter.TryParseValue(property.PropertySelector.ReturnType, parsedFilter.Value.Value, out _);
        }

        private static FilterOperator[] SupportedOperators(Type type)
        {
            return type switch
            {
                _ when type == typeof(string) =>
                [
                    FilterOperator.Equal, FilterOperator.NotEqual,
                    FilterOperator.Contains, FilterOperator.StartsWith
                ],
                _ when type == typeof(decimal) || type == typeof(int) || type == typeof(DateTime) =>
                [
                    FilterOperator.Equal, FilterOperator.NotEqual,
                    FilterOperator.GreaterThan, FilterOperator.GreaterThanOrEqual,
                    FilterOperator.LessThan, FilterOperator.LessThanOrEqual
                ],
                _ when type == typeof(bool) || type.IsEnum => [FilterOperator.Equal, FilterOperator.NotEqual],
                _ => []
            };
        }
    }
}