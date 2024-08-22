using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;

public abstract record CreateCustomFieldSetupRequest(
    string Name,
    string Description,
    CustomFieldType Type,
    CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest? NumberCustomFieldSetup,
    CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest? SingleSelectCustomFieldSetup,
    CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest? TextCustomFieldSetup)
{
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
}