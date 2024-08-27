using ConsiliumTempus.Domain.Common.Enums;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;

public sealed record UpdateCustomFieldSetupCommand(
    Guid Id,
    string Name,
    string Description,
    CustomFieldType Type,
    UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand? NumberCustomFieldSetup,
    UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand? SingleSelectCustomFieldSetup,
    UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand? TextCustomFieldSetup)
    : IRequest<ErrorOr<UpdateCustomFieldSetupResult>>
{
    public enum SingleSelectOptionOperation
    {
        Add,
        Update,
        Move,
        Remove,
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
        SingleSelectOptionOperation? Operation,
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
}