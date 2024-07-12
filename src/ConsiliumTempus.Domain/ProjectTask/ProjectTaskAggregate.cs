using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
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
    public UserAggregate? Assignee { get; private set; }
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

    public void Update(
        Name name,
        IsCompleted isCompleted,
        UserAggregate? assignee)
    {
        Name = name;
        IsCompleted = isCompleted;
        Assignee = assignee;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void UpdateOverview(
        Name name,
        Description description,
        IsCompleted isCompleted,
        UserAggregate? assignee)
    {
        Name = name;
        Description = description;
        IsCompleted = isCompleted;
        Assignee = assignee;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void UpdateCustomOrderPosition(CustomOrderPosition customOrderPosition)
    {
        CustomOrderPosition = customOrderPosition;
    }

    public void AddComment(ProjectTaskComment comment)
    {
        _comments.Add(comment);
    }

    public bool Move(Guid overId, List<ProjectStage> stages)
    {
        var overStage = stages.SingleOrDefault(s => s.Id.Value == overId);

        if (overStage is not null)
        {
            CustomOrderPosition = CustomOrderPosition.Create(0);
            Stage = overStage;
            overStage.AddTask(this);
        }
        else
        {
            overStage = stages
                .SelectMany(s => s.Tasks)
                .SingleOrDefault(t => t.Id.Value == overId)
                ?.Stage;

            if (overStage == Stage)
            {
                MoveWithinStage(ProjectTaskId.Create(overId));
            }
            else if (overStage is not null)
            {
                MoveInAnotherStage(ProjectTaskId.Create(overId), overStage);
            }
            else
            {
                return false;
            }
        }

        UpdatedDateTime = DateTime.UtcNow;

        return true;
    }

    private void MoveWithinStage(ProjectTaskId overProjectTaskId)
    {
        var overTask = Stage.Tasks.Single(t => t.Id == overProjectTaskId);

        var newCustomOrderPosition = CustomOrderPosition.Create(overTask.CustomOrderPosition.Value);

        if (CustomOrderPosition.Value < overTask.CustomOrderPosition.Value)
        {
            // task is placed on upper position
            for (var i = CustomOrderPosition.Value + 1; i <= overTask.CustomOrderPosition.Value; i++)
            {
                Stage.Tasks[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i - 1));
            }
        }
        else
        {
            // task is placed on lower position
            for (var i = overTask.CustomOrderPosition.Value + 1; i <= CustomOrderPosition.Value; i++)
            {
                Stage.Tasks[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i + 1));
            }
        }

        CustomOrderPosition = newCustomOrderPosition;
    }

    private void MoveInAnotherStage(ProjectTaskId overProjectTaskId, ProjectStage overStage)
    {
        var overTask = overStage.Tasks.Single(t => t.Id == overProjectTaskId);

        var newCustomOrderPosition = CustomOrderPosition.Create(overTask.CustomOrderPosition.Value + 1);

        Parallel.Invoke(
            () =>
            {
                // reorder new stage
                for (var i = overTask.CustomOrderPosition.Value + 1; i < overStage.Tasks.Count; i++)
                {
                    overStage.Tasks[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i + 1));
                }
            },
            () =>
            {
                // reorder old stage
                for (var i = CustomOrderPosition.Value + 1; i < Stage.Tasks.Count; i++)
                {
                    Stage.Tasks[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i + 1));
                }
            });

        Stage = overStage;
        CustomOrderPosition = newCustomOrderPosition;
    }
}