using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCustomFieldSetupResponse(GetCustomFieldSetupResponse.CustomFieldSetupResponse CustomFieldSetup)
{
    public record CustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description);

    public sealed record NumberCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        NumberCustomFieldSetupResponse.NumberCustomFieldSettingsResponse Settings,
        decimal? DefaultNumber)
        : CustomFieldSetupResponse(Id, Name, Description)
    {
        public sealed record NumberCustomFieldSettingsResponse(
            string CurrencyCode,
            short Decimals,
            bool Rounding);
    }

    public sealed record SingleSelectCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        List<SingleSelectCustomFieldSetupResponse.SingleSelectOptionResponse> Options,
        SingleSelectCustomFieldSetupResponse.SingleSelectOptionResponse? DefaultOption)
        : CustomFieldSetupResponse(Id, Name, Description)
    {
        public sealed record SingleSelectOptionResponse(
            Guid Id,
            string Value,
            string Color);
    }

    public sealed record TextCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        string? DefaultText)
        : CustomFieldSetupResponse(Id, Name, Description);
}