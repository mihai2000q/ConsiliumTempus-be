using System.Diagnostics.CodeAnalysis;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.Get;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetProjectTaskResponse(
    string Name,
    string Description,
    bool IsCompleted,
    GetProjectTaskResponse.UserResponse? Assignee,
    GetProjectTaskResponse.ProjectStageResponse Stage,
    GetProjectTaskResponse.ProjectSprintResponse Sprint,
    GetProjectTaskResponse.ProjectResponse Project,
    GetProjectTaskResponse.WorkspaceResponse Workspace)
{
    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);

    public sealed record ProjectStageResponse(
        Guid Id,
        string Name);

    public sealed record ProjectSprintResponse(
        Guid Id,
        string Name,
        List<ProjectStageResponse> Stages);

    public sealed record ProjectResponse(
        Guid Id,
        string Name);

    public sealed record WorkspaceResponse(
        Guid Id,
        string Name);
}