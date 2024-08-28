using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;

public abstract record CreateCustomFieldSetupRequest(
    string Name,
    string Description,
    CustomFieldType Type,
    CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest? DateCustomFieldSetup,
    CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest? DateTimeCustomFieldSetup,
    CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest? DurationCustomFieldSetup,
    CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest? MultiSelectCustomFieldSetup,
    CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? NumberCustomFieldSetup,
    CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? SingleSelectCustomFieldSetup,
    CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest? TextCustomFieldSetup,
    CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest? TimeCustomFieldSetup)
{
    public sealed record DateCustomFieldSetupRequest(
        DateOnly? DefaultDate);

    public sealed record DateTimeCustomFieldSetupRequest(
        DateTime? DefaultDateTime);
    
    public sealed record DurationCustomFieldSetupRequest(
        TimeSpan? DefaultDuration);
    
    public sealed record MultiSelectCustomFieldSetupRequest(
        List<MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest> Options)
    {
        public sealed record MultiSelectOptionRequest(
            string Value,
            string Color);
    }
    
    public sealed record NumberCustomFieldSetupRequest(
        NumberCustomFieldSetupRequest.NumberSettingsRequest Settings,
        decimal? DefaultNumber)
    {
        public sealed record NumberSettingsRequest(
            string? CurrencyCode,
            int Decimals,
            bool Rounding);
    }

    public sealed record SingleSelectCustomFieldSetupRequest(
        List<SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest> Options,
        string? DefaultOptionId)
    {
        public sealed record SingleSelectOptionRequest(
            string Id,
            string Value,
            string Color);
    }

    public sealed record TextCustomFieldSetupRequest(
        string? DefaultText);
    
    public sealed record TimeCustomFieldSetupRequest(
        TimeOnly? DefaultTime);
}