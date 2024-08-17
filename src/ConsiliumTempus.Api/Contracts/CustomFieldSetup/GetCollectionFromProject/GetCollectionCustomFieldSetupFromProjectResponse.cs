using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionCustomFieldSetupFromProjectResponse(
    List<GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse> CustomFieldSetups)
{
    public sealed record CustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        string Type);
}