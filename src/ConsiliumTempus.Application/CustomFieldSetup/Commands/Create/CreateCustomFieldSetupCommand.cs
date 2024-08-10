using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;

public sealed record CreateCustomFieldSetupCommand(
    Guid? WorkspaceId,
    Guid? ProjectId,
    string Name,
    string Description,
    string Type,
    CreateCustomFieldSetupCommand.NumberSettingsCommand? NumberSettings, 
    List<CreateCustomFieldSetupCommand.SingleSelectOptionCommand>? SingleSelectOptions)
    : IRequest<ErrorOr<CreateCustomFieldSetupResult>>
{
    public sealed record NumberSettingsCommand(
        string CurrencyCode,
        int Decimals,
        bool Rounding);

    public sealed record SingleSelectOptionCommand(
        string Value,
        string Color);
}