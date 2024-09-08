using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromWorkspace;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionCustomFieldSetupFromWorkspaceResponse(
    List<GetCollectionCustomFieldSetupFromWorkspaceResponse.CustomFieldSetupResponse> CustomFieldSetups)
{
    public sealed record CustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type);
}