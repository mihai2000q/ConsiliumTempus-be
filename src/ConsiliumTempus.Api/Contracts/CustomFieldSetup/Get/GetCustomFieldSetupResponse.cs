using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCustomFieldSetupResponse(GetCustomFieldSetupResponse.CustomFieldSetupResponse CustomFieldSetup)
{
    [JsonDerivedType(typeof(DateCustomFieldSetupResponse), nameof(DateCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(DateTimeCustomFieldSetupResponse), nameof(DateTimeCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(DurationCustomFieldSetupResponse), nameof(DurationCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(MultiSelectCustomFieldSetupResponse), nameof(MultiSelectCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(NumberCustomFieldSetupResponse), nameof(NumberCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(PeopleCustomFieldSetupResponse), nameof(PeopleCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(SingleSelectCustomFieldSetupResponse), nameof(SingleSelectCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(TextCustomFieldSetupResponse), nameof(TextCustomFieldSetupResponse))]
    [JsonDerivedType(typeof(TimeCustomFieldSetupResponse), nameof(TimeCustomFieldSetupResponse))]
    public abstract record CustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type);

    public sealed record DateCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        DateOnly? DefaultDate)
        : CustomFieldSetupResponse(Id, Name, Description, Type);

    public sealed record DateTimeCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        DateTime? DefaultDateTime)
        : CustomFieldSetupResponse(Id, Name, Description, Type);

    public sealed record DurationCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        TimeSpan? DefaultDuration)
        : CustomFieldSetupResponse(Id, Name, Description, Type);

    public sealed record MultiSelectCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        List<MultiSelectCustomFieldSetupResponse.MultiSelectOptionResponse> Options)
        : CustomFieldSetupResponse(Id, Name, Description, Type)
    {
        public sealed record MultiSelectOptionResponse(
            Guid Id,
            string Value,
            string Color);
    }

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

    public sealed record PeopleCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type)
        : CustomFieldSetupResponse(Id, Name, Description, Type);

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

    public sealed record TimeCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        TimeOnly? DefaultTime)
        : CustomFieldSetupResponse(Id, Name, Description, Type);
}