using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.ProjectSprint.Entities;

public sealed class ProjectStage : Entity<ProjectStageId>
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

    public Name Name { get; private set; } = default!;
    public CustomOrderPosition CustomOrderPosition { get; private set; } = default!;
    public ProjectSprintAggregate Sprint { get; init; } = default!;
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

    public void Update(Name name, CustomOrderPosition customOrderPosition)
    {
        Name = name;
        CustomOrderPosition = customOrderPosition;
    }

    public void AddTask(ProjectTaskAggregate task, bool onTop = false)
    {
        if (onTop)
        {
            _tasks.ForEach(t => 
                t.Update(t.Name, t.CustomOrderPosition + CustomOrderPosition.Create(1)));
            _tasks.Insert(0, task);
        }
        else
        {
            _tasks.Add(task);
        }
    }
    
    public void RemoveTask(ProjectTaskAggregate task)
    {
        if (!_tasks.Remove(task)) return;
        for (var i = task.CustomOrderPosition.Value; i < _tasks.Count; i++)
        {
            _tasks[i].DecrementCustomOrderPosition();
        }
    }
}