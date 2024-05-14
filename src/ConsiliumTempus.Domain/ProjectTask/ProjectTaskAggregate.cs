using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.ProjectTask;

public sealed class ProjectTaskAggregate : AggregateRoot<ProjectTaskId, Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectTaskAggregate()
    {
    }

    private ProjectTaskAggregate(
        ProjectTaskId id,
        Name name,
        Description description,
        IsCompleted isCompleted,
        CustomOrderPosition customOrderPosition,
        UserAggregate createdBy,
        ProjectStage stage,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        Name = name;
        Description = description;
        IsCompleted = isCompleted;
        CustomOrderPosition = customOrderPosition;
        CreatedBy = createdBy;
        Stage = stage;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    private readonly List<ProjectTaskComment> _comments = [];

    public Name Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public CustomOrderPosition CustomOrderPosition { get; private set; } = default!;
    public IsCompleted IsCompleted { get; private set; } = default!;
    public UserAggregate CreatedBy { get; init; } = default!;
    public UserAggregate? Asignee { get; private set; }
    public UserAggregate? Reviewer { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public TimeSpan? EstimatedDuration { get; private set; }
    public ProjectStage Stage { get; private set; } = default!;
    public IReadOnlyList<ProjectTaskComment> Comments => _comments.AsReadOnly();
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }

    public static ProjectTaskAggregate Create(
        Name name,
        Description description,
        CustomOrderPosition customOrderPosition,
        UserAggregate createdBy,
        ProjectStage stage)
    {
        return new ProjectTaskAggregate(
            ProjectTaskId.CreateUnique(),
            name,
            description,
            IsCompleted.Create(false),
            customOrderPosition,
            createdBy,
            stage,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }

    public void Update(Name name)
    {
        Name = name;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddComment(ProjectTaskComment comment)
    {
        _comments.Add(comment);
    }

    public void DecrementCustomOrderPosition()
    {
        CustomOrderPosition = CustomOrderPosition.Create(CustomOrderPosition.Value - 1);
    }
}