using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.CustomFieldSetup.GetCollectionFromProject;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionCustomFieldSetupFromProjectResponse(
    List<GetCollectionCustomFieldSetupFromProjectResponse.CustomFieldSetupResponse> CustomFieldSetups)
{
    public sealed record CustomFieldSetupResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type);
}