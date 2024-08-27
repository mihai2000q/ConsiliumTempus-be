using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Update;

public sealed record UpdateCustomFieldSetupRequest(
    Guid Id,
    string Name,
    string Description,
    CustomFieldType Type,
    UpdateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? NumberCustomFieldSetup,
    UpdateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? SingleSelectCustomFieldSetup,
    UpdateCustomFieldSetupRequest.TextCustomFieldSetupRequest? TextCustomFieldSetup)
{
    public enum SingleSelectOptionOperation
    {
        Add,
        Update,
        Move,
        Remove,
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
        SingleSelectOptionOperation? Operation,
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
}