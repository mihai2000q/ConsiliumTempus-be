using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.ProjectSprint.Entities;

public sealed class ProjectStage : Entity<ProjectStageId>, IAudit
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectStage()
    {
    }

    private ProjectStage(
        ProjectStageId id,
        Name name,
        CustomOrderPosition customOrderPosition,
        ProjectSprintAggregate sprint) : base(id)
    {
        Name = name;
        CustomOrderPosition = customOrderPosition;
        Sprint = sprint;
    }

    private readonly List<ProjectTaskAggregate> _tasks = [];

    public Name Name { get; private set; } = null!;
    public CustomOrderPosition CustomOrderPosition { get; private set; } = null!;
    public ProjectSprintAggregate Sprint { get; init; } = null!;
    public Audit Audit { get; init; } = null!;
    public IReadOnlyList<ProjectTaskAggregate> Tasks => _tasks.AsReadOnly();

    public static ProjectStage Create(
        Name name,
        CustomOrderPosition customOrderPosition,
        ProjectSprintAggregate sprint)
    {
        return new ProjectStage(
            ProjectStageId.CreateUnique(),
            name,
            customOrderPosition,
            sprint);
    }

    public void Update(Name name)
    {
        Name = name;
    }

    public void UpdateCustomOrderPosition(CustomOrderPosition customOrderPosition)
    {
        CustomOrderPosition = customOrderPosition;
    }

    public void AddTask(ProjectTaskAggregate task, bool onTop = false)
    {
        if (onTop)
        {
            // reorder
            _tasks.ForEach(t => t.UpdateCustomOrderPosition(t.CustomOrderPosition + 1));
            _tasks.Insert(0, task);
        }
        else
        {
            _tasks.Add(task);
        }
    }

    public void RemoveTask(ProjectTaskAggregate task, bool reorder = true)
    {
        if (reorder)
        {
            for (var i = task.CustomOrderPosition.Value + 1; i < _tasks.Count; i++)
            {
                _tasks[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i - 1));
            }
        }

        _tasks.Remove(task);
    }

    public ProjectStage CopyToSprint(ProjectSprintAggregate sprint)
    {
        return new ProjectStage(
            ProjectStageId.CreateUnique(),
            Name.Create(Name.Value),
            CustomOrderPosition.Create(CustomOrderPosition.Value),
            sprint);
    }

    public void Move(ProjectStage overStage)
    {
        var newCustomOrderPosition = CustomOrderPosition.Create(overStage.CustomOrderPosition.Value);

        if (CustomOrderPosition.Value < overStage.CustomOrderPosition.Value)
        {
            // stage is placed on upper position
            for (var pos = CustomOrderPosition + 1; pos <= overStage.CustomOrderPosition; pos++)
            {
                Sprint.Stages[pos.Value].UpdateCustomOrderPosition(pos - 1);
            }
        }
        else
        {
            // stage is placed on lower position
            for (var pos = overStage.CustomOrderPosition; pos < CustomOrderPosition; pos++)
            {
                Sprint.Stages[pos.Value].UpdateCustomOrderPosition(pos + 1);
            }
        }

        CustomOrderPosition = newCustomOrderPosition;
    }
}