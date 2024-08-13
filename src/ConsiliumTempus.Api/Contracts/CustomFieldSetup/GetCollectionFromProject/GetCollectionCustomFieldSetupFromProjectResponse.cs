using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionCustomFieldSetupFromProjectResponse(
    List<GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse> CustomFieldSetups)
{
    public abstract record CustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description);

    public sealed record NumberCustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        NumberCustomFieldSetupResponse.NumberCustomFieldSettingsResponse Settings)
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
        List<SingleSelectCustomFieldSetupResponse.SingleSelectOptionResponse> Options)
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
        string Description) 
        : CustomFieldSetupResponse(Id, Name, Description);
}