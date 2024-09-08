using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;

public sealed record UpdateCustomFieldSetupRequest(
    Guid Id,
    string Name,
    string Description,
    CustomFieldType Type,
    UpdateCustomFieldSetupRequest.DateCustomFieldSetupRequest? DateCustomFieldSetup,
    UpdateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest? DateTimeCustomFieldSetup,
    UpdateCustomFieldSetupRequest.DurationCustomFieldSetupRequest? DurationCustomFieldSetup,
    UpdateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest? MultiSelectCustomFieldSetup,
    UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? NumberCustomFieldSetup,
    UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? SingleSelectCustomFieldSetup,
    UpdateCustomFieldSetupRequest.TextCustomFieldSetupRequest? TextCustomFieldSetup,
    UpdateCustomFieldSetupRequest.TimeCustomFieldSetupRequest? TimeCustomFieldSetup)
{
    public enum OptionOperation
    {
        Add,
        Update,
        Move,
        Remove,
    }

    public sealed record DateCustomFieldSetupRequest(
        DateOnly? DefaultDate);

    public sealed record DateTimeCustomFieldSetupRequest(
        DateTime? DefaultDateTime);

    public sealed record DurationCustomFieldSetupRequest(
        TimeSpan? DefaultDuration);

    public sealed record MultiSelectCustomFieldSetupRequest(
        OptionOperation? Operation,
        MultiSelectCustomFieldSetupRequest.MultiSelectOptionRequest? NewOption,
        Guid? OptionId,
        Guid? OverOptionId)
    {
        public sealed record MultiSelectOptionRequest(
            string Color,
            string Value);
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
        Guid? DefaultOptionId,
        OptionOperation? Operation,
        SingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest? NewOption,
        Guid? OptionId,
        Guid? OverOptionId)
    {
        public sealed record SingleSelectOptionRequest(
            string Color,
            string Value);
    }

    public sealed record TextCustomFieldSetupRequest(
        string? DefaultText);

    public sealed record TimeCustomFieldSetupRequest(
        TimeOnly? DefaultTime);
}