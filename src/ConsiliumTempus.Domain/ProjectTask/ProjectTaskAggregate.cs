using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Extensions;
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

    public bool Move(Guid overId)
    {
        var overStage = Stage.Sprint.Stages.SingleOrDefault(s => s.Id.Value == overId);

        if (overStage is not null)
        {
            Parallel.Invoke(
                () =>
                {
                    ReorderStage();
                    Stage.RemoveTask(this, false);
                },
                () => overStage.AddTask(this, true));

            CustomOrderPosition = CustomOrderPosition.Create(0);
            Stage = overStage;
        }
        else
        {
            overStage = Stage.Sprint.Stages
                .SelectMany(s => s.Tasks)
                .SingleOrDefault(t => t.Id.Value == overId)
                ?.Stage;

            var overTaskId = ProjectTaskId.Create(overId);

            if (overStage == Stage) MoveWithinStage(overTaskId);
            else if (overStage is not null) MoveToAnotherStage(overTaskId, overStage);
            else return true;
        }

        UpdatedDateTime = DateTime.UtcNow;

        return false;
    }

    private void MoveWithinStage(ProjectTaskId overTaskId)
    {
        var overTask = Stage.Tasks.Single(t => t.Id == overTaskId);

        var newCustomOrderPosition = CustomOrderPosition.Create(overTask.CustomOrderPosition.Value);

        var (start, end, sign) = CustomOrderPosition.Value < overTask.CustomOrderPosition.Value
            ? (CustomOrderPosition.Value + 1, overTask.CustomOrderPosition.Value + 1, -1)
            : (overTask.CustomOrderPosition.Value, CustomOrderPosition.Value, 1);

        Stage.Tasks
            .OrderBy(t => t.CustomOrderPosition.Value)
            .Skip(start)
            .Take(end - start)
            .ForEach(t => t.UpdateCustomOrderPosition(t.CustomOrderPosition + sign));

        CustomOrderPosition = newCustomOrderPosition;
    }

    private void MoveToAnotherStage(ProjectTaskId overTaskId, ProjectStage overStage)
    {
        var overTask = overStage.Tasks.Single(t => t.Id == overTaskId);

        var newCustomOrderPosition = CustomOrderPosition.Create(overTask.CustomOrderPosition.Value);

        Parallel.Invoke(
            () =>
            {
                // reorder new stage
                for (var i = overTask.CustomOrderPosition.Value; i < overStage.Tasks.Count; i++)
                {
                    overStage.Tasks[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i + 1));
                }
                overStage.AddTask(this);
            },
            () =>
            {
                ReorderStage();
                Stage.RemoveTask(this, false);
            });

        Stage = overStage;
        CustomOrderPosition = newCustomOrderPosition;
    }

    /// <summary>
    /// Orders the tasks inside the stage by Custom Order Position and then updates all the tasks' position.
    /// </summary>
    /// <remarks>
    /// Current Stage is not ordered even before deletion, therefore we order it manually by Custom Order Position,
    /// and then we can update the position for all tasks
    /// </remarks>
    private void ReorderStage()
    {
        Stage.Tasks
            .OrderBy(t => t.CustomOrderPosition.Value)
            .Skip(CustomOrderPosition.Value + 1)
            .ForEach(t => t.UpdateCustomOrderPosition(t.CustomOrderPosition - 1));
    }
}