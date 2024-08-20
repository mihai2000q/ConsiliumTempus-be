using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCustomFieldSetupResponse(GetCustomFieldSetupResponse.CustomFieldSetupResponse CustomFieldSetup)
{
    public abstract record CustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description, 
        string Type);

    public sealed record NumberCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        string Type,
        NumberCustomFieldSetupResponse.NumberCustomFieldSettingsResponse Settings,
        decimal? DefaultNumber)
        : CustomFieldSetupResponse(Id, Name, Description, Type)
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
        string Type,
        List<SingleSelectCustomFieldSetupResponse.SingleSelectOptionResponse> Options,
        SingleSelectCustomFieldSetupResponse.SingleSelectOptionResponse? DefaultOption)
        : CustomFieldSetupResponse(Id, Name, Description, Type)
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
        string Type,
        string? DefaultText)
        : CustomFieldSetupResponse(Id, Name, Description, Type);
}