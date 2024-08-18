using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Update;

public sealed record UpdateCustomFieldSetupCommand(
    Guid Id,
    string Name,
    string Description,
    string Type,
    UpdateCustomFieldSetupCommand.NumberCustomFieldSetupCommand? NumberCustomFieldSetup,
    UpdateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand? SingleSelectCustomFieldSetup,
    UpdateCustomFieldSetupCommand.TextCustomFieldSetupCommand? TextCustomFieldSetup)
    : IRequest<ErrorOr<UpdateCustomFieldSetupResult>>
{
    public sealed record NumberCustomFieldSetupCommand(
        NumberCustomFieldSetupCommand.NumberSettingsCommand Settings,
        decimal? DefaultNumber)
    {
        public sealed record NumberSettingsCommand(
            string CurrencyCode,
            int Decimals,
            bool Rounding);
    }

    public sealed record SingleSelectCustomFieldSetupCommand(Guid? DefaultOptionId);

    public sealed record TextCustomFieldSetupCommand(
        string? DefaultText);
}