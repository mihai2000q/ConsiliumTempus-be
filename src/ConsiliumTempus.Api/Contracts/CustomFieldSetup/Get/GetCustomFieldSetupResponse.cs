using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[JsonDerivedType(typeof(GetDateCustomFieldSetupResponse))]
[JsonDerivedType(typeof(GetDateTimeCustomFieldSetupResponse))]
[JsonDerivedType(typeof(GetDurationCustomFieldSetupResponse))]
[JsonDerivedType(typeof(GetMultiSelectCustomFieldSetupResponse))]
[JsonDerivedType(typeof(GetNumberCustomFieldSetupResponse))]
[JsonDerivedType(typeof(GetPeopleCustomFieldSetupResponse))]
[JsonDerivedType(typeof(GetSingleSelectCustomFieldSetupResponse))]
[JsonDerivedType(typeof(GetTextCustomFieldSetupResponse))]
[JsonDerivedType(typeof(GetTimeCustomFieldSetupResponse))]
public abstract record GetCustomFieldSetupResponse(
    string Name,
    string Description,
    CustomFieldType Type)
{
    public sealed record GetDateCustomFieldSetupResponse(
        string Name,
        string Description,
        CustomFieldType Type,
        DateOnly? DefaultDate)
        : GetCustomFieldSetupResponse(Name, Description, Type);

    public sealed record GetDateTimeCustomFieldSetupResponse(
        string Name,
        string Description,
        CustomFieldType Type,
        DateTime? DefaultDateTime)
        : GetCustomFieldSetupResponse(Name, Description, Type);

    public sealed record GetDurationCustomFieldSetupResponse(
        string Name,
        string Description,
        CustomFieldType Type,
        TimeSpan? DefaultDuration)
        : GetCustomFieldSetupResponse(Name, Description, Type);

    public sealed record GetMultiSelectCustomFieldSetupResponse(
        string Name,
        string Description,
        CustomFieldType Type,
        List<GetMultiSelectCustomFieldSetupResponse.MultiSelectOptionResponse> Options)
        : GetCustomFieldSetupResponse(Name, Description, Type)
    {
        public sealed record MultiSelectOptionResponse(
            Guid Id,
            string Value,
            string Color);
    }

    public sealed record GetNumberCustomFieldSetupResponse(
        string Name,
        string Description,
        CustomFieldType Type,
        GetNumberCustomFieldSetupResponse.NumberCustomFieldSettingsResponse Settings,
        decimal? DefaultNumber)
        : GetCustomFieldSetupResponse(Name, Description, Type)
    {
        public sealed record NumberCustomFieldSettingsResponse(
            string CurrencyCode,
            short Decimals,
            bool Rounding);
    }

    public sealed record GetPeopleCustomFieldSetupResponse(
        string Name,
        string Description,
        CustomFieldType Type)
        : GetCustomFieldSetupResponse(Name, Description, Type);

    public sealed record GetSingleSelectCustomFieldSetupResponse(
        string Name,
        string Description,
        CustomFieldType Type,
        List<GetSingleSelectCustomFieldSetupResponse.SingleSelectOptionResponse> Options,
        GetSingleSelectCustomFieldSetupResponse.SingleSelectOptionResponse? DefaultOption)
        : GetCustomFieldSetupResponse(Name, Description, Type)
    {
        public sealed record SingleSelectOptionResponse(
            Guid Id,
            string Value,
            string Color);
    }

    public sealed record GetTextCustomFieldSetupResponse(
        string Name,
        string Description,
        CustomFieldType Type,
        string? DefaultText)
        : GetCustomFieldSetupResponse(Name, Description, Type);

    public sealed record GetTimeCustomFieldSetupResponse(
        string Name,
        string Description,
        CustomFieldType Type,
        TimeOnly? DefaultTime)
        : GetCustomFieldSetupResponse(Name, Description, Type);
}