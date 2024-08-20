using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ConsiliumTempus.Domain.Common.Enums;

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
    GetProjectTaskResponse.WorkspaceResponse Workspace,
    List<GetProjectTaskResponse.CustomFieldResponse> CustomFields)
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
    
    [JsonDerivedType(typeof(NumberCustomFieldResponse), nameof(NumberCustomFieldResponse))]
    [JsonDerivedType(typeof(SingleSelectCustomFieldResponse), nameof(SingleSelectCustomFieldResponse))]
    [JsonDerivedType(typeof(TextCustomFieldResponse), nameof(TextCustomFieldResponse))]
    public abstract record CustomFieldResponse(
        Guid Id,
        CustomFieldType Type);

    public sealed record NumberCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        decimal? Number)
        : CustomFieldResponse(Id, Type);

    public sealed record SingleSelectCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        SingleSelectCustomFieldResponse.SingleSelectOptionResponse? Option,
        List<SingleSelectCustomFieldResponse.SingleSelectOptionResponse> AvailableOptions)
        : CustomFieldResponse(Id, Type)
    {
        public sealed record SingleSelectOptionResponse(
            Guid Id,
            string Value,
            string Color);
    };

    public sealed record TextCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        string? Text)
        : CustomFieldResponse(Id, Type);
}