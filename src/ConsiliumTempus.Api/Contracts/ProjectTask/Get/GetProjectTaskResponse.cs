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

    [JsonDerivedType(typeof(DateCustomFieldResponse), nameof(DateCustomFieldResponse))]
    [JsonDerivedType(typeof(DateTimeCustomFieldResponse), nameof(DateTimeCustomFieldResponse))]
    [JsonDerivedType(typeof(DurationCustomFieldResponse), nameof(DurationCustomFieldResponse))]
    [JsonDerivedType(typeof(MultiSelectCustomFieldResponse), nameof(MultiSelectCustomFieldResponse))]
    [JsonDerivedType(typeof(NumberCustomFieldResponse), nameof(NumberCustomFieldResponse))]
    [JsonDerivedType(typeof(PeopleCustomFieldResponse), nameof(PeopleCustomFieldResponse))]
    [JsonDerivedType(typeof(SingleSelectCustomFieldResponse), nameof(SingleSelectCustomFieldResponse))]
    [JsonDerivedType(typeof(TextCustomFieldResponse), nameof(TextCustomFieldResponse))]
    [JsonDerivedType(typeof(TimeCustomFieldResponse), nameof(TimeCustomFieldResponse))]
    public abstract record CustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type);

    public sealed record DateCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        DateOnly? Date)
        : CustomFieldResponse(Id, Name, Description, Type);

    public sealed record DateTimeCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        DateTime? DateTime)
        : CustomFieldResponse(Id, Name, Description, Type);

    public sealed record DurationCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        TimeSpan? Duration)
        : CustomFieldResponse(Id, Name, Description, Type);

    public sealed record MultiSelectCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        List<MultiSelectCustomFieldResponse.MultiSelectOptionResponse> Options,
        List<MultiSelectCustomFieldResponse.MultiSelectOptionResponse> AvailableOptions)
        : CustomFieldResponse(Id, Name, Description, Type)
    {
        public sealed record MultiSelectOptionResponse(
            Guid Id,
            string Value,
            string Color);
    };

    public sealed record NumberCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        decimal? Number)
        : CustomFieldResponse(Id, Name, Description, Type);

    public sealed record PeopleCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        UserResponse? Person)
        : CustomFieldResponse(Id, Name, Description, Type);

    public sealed record SingleSelectCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        SingleSelectCustomFieldResponse.SingleSelectOptionResponse? Option,
        List<SingleSelectCustomFieldResponse.SingleSelectOptionResponse> AvailableOptions)
        : CustomFieldResponse(Id, Name, Description, Type)
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
        : CustomFieldResponse(Id, Name, Description, Type);

    public sealed record TimeCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        CustomFieldType Type,
        TimeOnly? Time)
        : CustomFieldResponse(Id, Name, Description, Type);
}