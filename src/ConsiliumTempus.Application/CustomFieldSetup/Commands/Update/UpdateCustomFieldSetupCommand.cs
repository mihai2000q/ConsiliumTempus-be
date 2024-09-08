using ConsiliumTempus.Domain.Common.Enums;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;

public sealed record UpdateCustomFieldSetupCommand(
    Guid Id,
    string Name,
    string Description,
    CustomFieldType Type,
    UpdateCustomFieldSetupCommand.DateCustomFieldSetupCommand? DateCustomFieldSetup,
    UpdateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand? DateTimeCustomFieldSetup,
    UpdateCustomFieldSetupCommand.DurationCustomFieldSetupCommand? DurationCustomFieldSetup,
    UpdateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand? MultiSelectCustomFieldSetup,
    UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand? NumberCustomFieldSetup,
    UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand? SingleSelectCustomFieldSetup,
    UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand? TextCustomFieldSetup,
    UpdateCustomFieldSetupCommand.TimeCustomFieldSetupCommand? TimeCustomFieldSetup)
    : IRequest<ErrorOr<UpdateCustomFieldSetupResult>>
{
    public enum OptionOperation
    {
        Add,
        Update,
        Move,
        Remove,
    }

    public sealed record DateCustomFieldSetupCommand(
        DateOnly? DefaultDate);

    public sealed record DateTimeCustomFieldSetupCommand(
        DateTime? DefaultDateTime);

    public sealed record DurationCustomFieldSetupCommand(
        TimeSpan? DefaultDuration);

    public sealed record MultiSelectCustomFieldSetupCommand(
        OptionOperation? Operation,
        MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand? NewOption,
        Guid? OptionId,
        Guid? OverOptionId)
    {
        public sealed record MultiSelectOptionCommand(
            string Color,
            string Value);
    }

    public sealed record NumberCustomFieldSetupCommand(
        NumberCustomFieldSetupCommand.NumberSettingsCommand Settings,
        decimal? DefaultNumber)
    {
        public sealed record NumberSettingsCommand(
            string? CurrencyCode,
            int Decimals,
            bool Rounding);
    }

    public sealed record SingleSelectCustomFieldSetupCommand(
        Guid? DefaultOptionId,
        OptionOperation? Operation,
        SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand? NewOption,
        Guid? OptionId,
        Guid? OverOptionId)
    {
        public sealed record SingleSelectOptionCommand(
            string Color,
            string Value);
    }

    public sealed record TextCustomFieldSetupCommand(
        string? DefaultText);

    public sealed record TimeCustomFieldSetupCommand(
        TimeOnly? DefaultTime);
}