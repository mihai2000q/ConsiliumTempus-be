using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ConsiliumTempus.Domain.Common.Enums;

namespace ConsiliumTempus.Api.Contracts.ProjectTask.GetCollection;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed record GetCollectionProjectTaskResponse(
    List<GetCollectionProjectTaskResponse.ProjectTaskResponse> Tasks,
    int TotalCount)
{
    public sealed record ProjectTaskResponse(
        Guid Id,
        string Name,
        string Description,
        bool IsCompleted,
        UserResponse? Assignee,
        List<CustomFieldResponse> CustomFields);

    public sealed record UserResponse(
        Guid Id,
        string Name,
        string Email);

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
        SingleSelectCustomFieldResponse.SingleSelectOptionResponse? Option)
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