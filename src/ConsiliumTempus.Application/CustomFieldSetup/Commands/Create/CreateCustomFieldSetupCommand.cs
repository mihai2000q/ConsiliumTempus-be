using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;

public sealed record CreateCustomFieldSetupCommand(
    Guid? WorkspaceId,
    Guid? ProjectId,
    string Name,
    string Description,
    string Type,
    CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand? NumberCustomFieldSetup,
    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand? SingleSelectCustomFieldSetup,
    CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand? TextCustomFieldSetup)
    : IRequest<ErrorOr<CreateCustomFieldSetupResult>>
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

    public sealed record SingleSelectCustomFieldSetupCommand(
        List<SingleSelectCustomFieldSetupCommand.SingleSelectOptionCommand> Options,
        string? DefaultOptionId)
    {
        public sealed record SingleSelectOptionCommand(
            string Id,
            string Value,
            string Color);
    }

    public sealed record TextCustomFieldSetupCommand(
        string? DefaultText);
}