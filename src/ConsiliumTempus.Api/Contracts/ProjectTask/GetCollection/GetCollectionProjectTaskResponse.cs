using System.Diagnostics.CodeAnalysis;

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
    
    public abstract record CustomFieldResponse(
        Guid Id,
        string Type);

    public sealed record NumberCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        string Type,
        decimal? Number) 
        : CustomFieldResponse(Id, Type);

    public sealed record SingleSelectCustomFieldResponse(
        Guid Id,
        string Name,
        string Description,
        string Type,
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
        string Type,
        string? Text) 
        : CustomFieldResponse(Id, Type);
}