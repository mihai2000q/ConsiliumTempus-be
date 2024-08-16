using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;

public sealed record CreateCustomFieldSetupCommand(
    Guid? WorkspaceId,
    Guid? ProjectId,
    string Name,
    string Description,
    string Type,
    CreateCustomFieldSetupCommand.CreateNumberCustomFieldSetupCommand? NumberCustomFieldSetup,
    CreateCustomFieldSetupCommand.CreateSingleSelectCustomFieldSetupCommand? SingleSelectCustomFieldSetup,
    CreateCustomFieldSetupCommand.CreateTextCustomFieldSetupCommand? TextCustomFieldSetup)
    : IRequest<ErrorOr<CreateCustomFieldSetupResult>>
{
    public sealed record CreateNumberCustomFieldSetupCommand(
        CreateNumberCustomFieldSetupCommand.NumberSettingsCommand Settings,
        decimal? DefaultNumber)
    {
        public sealed record NumberSettingsCommand(
            string CurrencyCode,
            int Decimals,
            bool Rounding);
    }

    public sealed record CreateSingleSelectCustomFieldSetupCommand(
        List<CreateSingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand> Options,
        string? DefaultOptionId)
    {
        public sealed record SingleSelectOptionCommand(
            string Id,
            string Value,
            string Color);
    }

    public sealed record CreateTextCustomFieldSetupCommand(
        string? DefaultText);
}