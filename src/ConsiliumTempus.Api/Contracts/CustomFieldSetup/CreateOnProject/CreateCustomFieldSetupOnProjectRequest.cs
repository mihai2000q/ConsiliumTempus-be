namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;

public sealed record CreateCustomFieldSetupOnProjectRequest(
    Guid? ProjectId,
    string Name,
    string Description,
    string Type,
    CreateCustomFieldSetupOnProjectRequest.CreateNumberCustomFieldSetupRequest? NumberCustomFieldSetup,
    CreateCustomFieldSetupOnProjectRequest.CreateSingleSelectCustomFieldSetupRequest? SingleSelectCustomFieldSetup,
    CreateCustomFieldSetupOnProjectRequest.CreateTextCustomFieldSetupRequest? TextCustomFieldSetup)
{
    public sealed record CreateNumberCustomFieldSetupRequest(
        CreateNumberCustomFieldSetupRequest.NumberSettingsRequest Settings,
        decimal? DefaultNumber)
    {
        public sealed record NumberSettingsRequest(
            string CurrencyCode,
            int Decimals,
            bool Rounding);
    }

    public sealed record CreateSingleSelectCustomFieldSetupRequest(
        List<CreateSingleSelectCustomFieldSetupRequest.SingleSelectOptionRequest> Options,
        string? DefaultOptionId)
    {
        public sealed record SingleSelectOptionRequest(
            string Id,
            string Value,
            string Color);
    }

    public sealed record CreateTextCustomFieldSetupRequest(
        string? DefaultText);
}