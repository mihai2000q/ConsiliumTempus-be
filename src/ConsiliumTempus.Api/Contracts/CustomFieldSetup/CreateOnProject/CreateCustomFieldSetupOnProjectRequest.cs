namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;

public sealed record CreateCustomFieldSetupOnProjectRequest(
    Guid? ProjectId,
    string Name,
    string Description,
    string Type,
    CreateCustomFieldSetupOnProjectRequest.NumberSettingsRequest? NumberSettings,
    List<CreateCustomFieldSetupOnProjectRequest.SingleSelectOptionRequest>? SingleSelectOptions)
{
    public sealed record NumberSettingsRequest(
        string CurrencyCode,
        int Decimals,
        bool Rounding);

    public sealed record SingleSelectOptionRequest(
        string Value,
        string Color);
}