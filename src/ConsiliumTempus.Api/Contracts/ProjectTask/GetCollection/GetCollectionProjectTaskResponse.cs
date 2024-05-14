using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectTaskResponse(
    List<GetCollectionProjectTaskResponse.ProjectTaskResponse> Tasks,
    int TotalCount)
{
    public sealed record ProjectTaskResponse(
        Guid Id,
        string Name);
}