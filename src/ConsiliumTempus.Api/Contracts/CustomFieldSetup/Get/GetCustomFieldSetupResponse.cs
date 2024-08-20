using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCustomFieldSetupResponse(GetCustomFieldSetupResponse.CustomFieldSetupResponse CustomFieldSetup)
{
    [JsonDerivedType(typeof(NumberCustomFieldSetupResponse), nameof(NumberCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(SingleSelectCustomFieldSetupResponse), nameof(SingleSelectCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(TextCustomFieldSetupResponse), nameof(TextCustomFieldSetupResponse))]
    public abstract record CustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type);

    public sealed record NumberCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
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
        CustomFieldType Type,
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
        CustomFieldType Type,
        string? DefaultText)
        : CustomFieldSetupResponse(Id, Name, Description, Type);
}