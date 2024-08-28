using ConsiliumTempus.Domain.Common.Enums;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.CustomFieldSetup.Commands.Create;

public sealed record CreateCustomFieldSetupCommand(
    Guid? WorkspaceId,
    Guid? ProjectId,
    string Name,
    string Description,
    CustomFieldType Type,
    CreateCustomFieldSetupCommand.DateCustomFieldSetupCommand? DateCustomFieldSetup,
    CreateCustomFieldSetupCommand.DateTimeCustomFieldSetupCommand? DateTimeCustomFieldSetup,
    CreateCustomFieldSetupCommand.DurationCustomFieldSetupCommand? DurationCustomFieldSetup,
    CreateCustomFieldSetupCommand.MultiSelectCustomFieldSetupCommand? MultiSelectCustomFieldSetup,
    CreateCustomFieldSetupCommand.NumberCustomFieldSetupCommand? NumberCustomFieldSetup,
    CreateCustomFieldSetupCommand.SingleSelectCustomFieldSetupCommand? SingleSelectCustomFieldSetup,
    CreateCustomFieldSetupCommand.TextCustomFieldSetupCommand? TextCustomFieldSetup,
    CreateCustomFieldSetupCommand.TimeCustomFieldSetupCommand? TimeCustomFieldSetup)
    : IRequest<ErrorOr<CreateCustomFieldSetupResult>>
{
    public sealed record DateCustomFieldSetupCommand(
        DateOnly? DefaultDate);

    public sealed record DateTimeCustomFieldSetupCommand(
        DateTime? DefaultDateTime);
    
    public sealed record DurationCustomFieldSetupCommand(
        TimeSpan? DefaultDuration);
    
    public sealed record MultiSelectCustomFieldSetupCommand(
        List<MultiSelectCustomFieldSetupCommand.MultiSelectOptionCommand> Options)
    {
        public sealed record MultiSelectOptionCommand(
            string Value,
            string Color);
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
    
    public sealed record TimeCustomFieldSetupCommand(
        TimeOnly? DefaultTime);
}