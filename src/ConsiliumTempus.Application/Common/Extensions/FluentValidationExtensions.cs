using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.Enums;
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

    public static IRuleBuilderOptions<T, string> IsWorkspaceRole<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(wr => WorkspaceRole.FromName(wr.Capitalize()) is not null)
            .WithMessage("'{PropertyName}' must be valid workspace role");
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

    public static IRuleBuilderOptions<T, string[]?> HasSearchFormat<T, TEntity>(
        this IRuleBuilder<T, string[]?> ruleBuilder,
        IReadOnlyList<FilterProperty<TEntity>> filterProperties)
    {
        return ruleBuilder
            .Must(search => search.ForAll(SearchValidation.SeparatorValidation))
            .WithMessage(
                "The elements of '{PropertyName}' must have the following format: {Property} {Operator} {Value} " +
                "(separated by whitespaces)")
            .Must(search => search.ForAll(SearchValidation.SnakeCaseValidation))
            .WithMessage(
                "The elements of '{PropertyName}' must have the following format: {Property} {Operator} {Value}, " +
                "where 'Property' must be in snake case")
            .Must(search => search.ForAll(f => SearchValidation.PropertyValidation(f, filterProperties)))
            .WithMessage(
                "The elements of '{PropertyName}' must have the following format: {Property} {Operator} {Value}, " +
                $"where 'Property' must be a property of the entity: {typeof(TEntity).Name} " +
                "(available properties: " +
                $"{string.Join(", ", filterProperties.Select(x => x.Identifier))})")
            .Must(search => search.ForAll(SearchValidation.OperatorValidation))
            .WithMessage(
                "The elements of '{PropertyName}' must have the following format: {Property} {Operator} {Value}, " +
                "where 'Operator' must be one of the following: " +
                $"{string.Join(", ", Filter.OperatorToFilterOperator.Keys)}")
            .Must(search => search.ForAll(f => SearchValidation.OperatorAndValueTypeValidation(f, filterProperties)))
            .WithMessage("The '{PropertyName}' must have a supported combination of Type and Operator")
            .Must(search => search.ForAll(f => SearchValidation.ValueParsingValidation(f, filterProperties)))
            .WithMessage(
                "The Value from the '{PropertyName}', given the type of the filter proposed, cannot be parsed");
    }

    private static bool ForAll(this string[]? str, Func<string, bool> validator) => str is null || str.All(validator);

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

    private static class SearchValidation
    {
        internal static bool SeparatorValidation(string filter)
        {
            return SplitFilter(filter) is not null;
        }

        internal static bool SnakeCaseValidation(string filter)
        {
            var parsedFilter = SplitFilter(filter);
            return parsedFilter is null ||
                   !parsedFilter.Value.PropertyIdentifier.Any(char.IsUpper);
        }

        internal static bool PropertyValidation<TEntity>(
            string filter,
            IReadOnlyList<FilterProperty<TEntity>> filterProperties)
        {
            var parsedFilter = SplitFilter(filter);
            if (parsedFilter is null) return true;

            var property = filterProperties
                .SingleOrDefault(fp => fp.Identifier == parsedFilter.Value.PropertyIdentifier);
            return parsedFilter.Value.PropertyIdentifier.Any(char.IsUpper) ||
                   property is not null;
        }

        internal static bool OperatorValidation(string filter)
        {
            var parsedFilter = SplitFilter(filter);
            return parsedFilter is null ||
                   Filter.OperatorToFilterOperator.TryGetValue(parsedFilter.Value.Operator, out _);
        }

        internal static bool OperatorAndValueTypeValidation<TEntity>(
            string filter,
            IReadOnlyList<FilterProperty<TEntity>> filterProperties)
        {
            var parsedFilter = SplitFilter(filter);
            if (parsedFilter is null) return true;

            var property = filterProperties
                .SingleOrDefault(fp => fp.Identifier == parsedFilter.Value.PropertyIdentifier);

            return property is null ||
                   !Filter.OperatorToFilterOperator.TryGetValue(parsedFilter.Value.Operator, out var filterOperator) ||
                   (SupportedOperatorsByType.ContainsKey(property.PropertySelector.ReturnType) &&
                    SupportedOperatorsByType[property.PropertySelector.ReturnType].Contains(filterOperator));
        }

        internal static bool ValueParsingValidation<TEntity>(
            string filter,
            IReadOnlyList<FilterProperty<TEntity>> filterProperties)
        {
            var parsedFilter = SplitFilter(filter);
            if (parsedFilter is null) return true;

            var property = filterProperties
                .SingleOrDefault(fp => fp.Identifier == parsedFilter.Value.PropertyIdentifier);
            if (property is null || !SupportedOperatorsByType.ContainsKey(property.PropertySelector.ReturnType))
                return true;

            var type = property.PropertySelector.ReturnType;
            return type switch
            {
                not null when type == typeof(bool) => bool.TryParse(parsedFilter.Value.Value, out _),
                not null when type == typeof(DateTime) => DateTime.TryParse(parsedFilter.Value.Value, out _),
                not null when type == typeof(decimal) => decimal.TryParse(parsedFilter.Value.Value, out _),
                not null when type == typeof(int) => int.TryParse(parsedFilter.Value.Value, out _),
                not null when type == typeof(ProjectLifecycle) => 
                    Enum.TryParse<ProjectLifecycle>(parsedFilter.Value.Value, true, out _),
                not null when type == typeof(ProjectStatusType) => 
                    Enum.TryParse<ProjectStatusType>(parsedFilter.Value.Value, true, out _),
                _ => true
            };
        }

        private static (string PropertyIdentifier, string Operator, string Value)? SplitFilter(string filter)
        {
            filter = filter.Trim();
            var result = new List<string>();

            var current = "";
            for (var i = 0; i < filter.Length; i++)
                if (filter[i] == Filter.Separator)
                {
                    result.Add(current);
                    current = "";
                    if (result.Count != 2 || i == filter.Length - 1) continue;
                    result.Add(filter[(i + 1)..]);
                    break;
                }
                else
                    current += filter[i];

            if (result.Count != 3) return null;

            return (result[0], result[1], result[2]);
        }

        private static readonly Dictionary<Type, List<FilterOperator>> SupportedOperatorsByType = new()
        {
            {
                typeof(string), [
                    FilterOperator.Equal, FilterOperator.NotEqual,
                    FilterOperator.Contains, FilterOperator.StartsWith
                ]
            },
            {
                typeof(decimal), [
                    FilterOperator.Equal, FilterOperator.NotEqual,
                    FilterOperator.GreaterThan, FilterOperator.GreaterThanOrEqual,
                    FilterOperator.LessThan, FilterOperator.LessThanOrEqual
                ]
            },
            {
                typeof(int), [
                    FilterOperator.Equal, FilterOperator.NotEqual,
                    FilterOperator.GreaterThan, FilterOperator.GreaterThanOrEqual,
                    FilterOperator.LessThan, FilterOperator.LessThanOrEqual
                ]
            },
            {
                typeof(DateTime),
                [
                    FilterOperator.Equal, FilterOperator.NotEqual,
                    FilterOperator.GreaterThan, FilterOperator.GreaterThanOrEqual,
                    FilterOperator.LessThan, FilterOperator.LessThanOrEqual
                ]
            },
            { typeof(bool), [FilterOperator.Equal, FilterOperator.NotEqual] },
            { typeof(ProjectLifecycle), [FilterOperator.Equal, FilterOperator.NotEqual] },
            { typeof(ProjectStatusType), [FilterOperator.Equal, FilterOperator.NotEqual] }
        };
    }
}