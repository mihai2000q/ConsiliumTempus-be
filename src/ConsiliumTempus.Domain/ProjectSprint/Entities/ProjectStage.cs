using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;
using ConsiliumTempus.Domain.User;

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
        ProjectSprintAggregate sprint,
        Audit audit) : base(id)
    {
        Name = name;
        CustomOrderPosition = customOrderPosition;
        Sprint = sprint;
        Audit = audit;
    }

    private readonly List<ProjectTaskAggregate> _tasks = [];

    public Name Name { get; private set; } = default!;
    public CustomOrderPosition CustomOrderPosition { get; private set; } = default!;
    public ProjectSprintAggregate Sprint { get; init; } = default!;
    public Audit Audit { get; init; } = default!;
    public IReadOnlyList<ProjectTaskAggregate> Tasks => _tasks.AsReadOnly();

    public static ProjectStage Create(
        Name name,
        CustomOrderPosition customOrderPosition,
        ProjectSprintAggregate sprint,
        UserAggregate createdBy)
    {
        return new ProjectStage(
            ProjectStageId.CreateUnique(),
            name,
            customOrderPosition,
            sprint,
            Audit.Create(createdBy));
    }

    public void Update(
        Name name, 
        CustomOrderPosition customOrderPosition,
        UserAggregate updatedBy)
    {
        Name = name;
        CustomOrderPosition = customOrderPosition;
        Audit.Update(updatedBy);
    }
    
    public void UpdateWithoutAudit(Name name, CustomOrderPosition customOrderPosition)
    {
        Name = name;
        CustomOrderPosition = customOrderPosition;
    }

    public void AddTask(ProjectTaskAggregate task, bool onTop = false)
    {
        if (onTop)
        {
            _tasks.ForEach(t => 
                t.UpdateCustomOrderPosition(t.CustomOrderPosition + CustomOrderPosition.Create(1)));
            _tasks.Insert(0, task);
        }
        else
        {
            _tasks.Add(task);
        }
    }

    public void RemoveTask(ProjectTaskAggregate task)
    {
        _tasks.Remove(task);
        for (var i = task.CustomOrderPosition.Value; i < _tasks.Count; i++)
        {
            _tasks[i].UpdateCustomOrderPosition(_tasks[i].CustomOrderPosition - CustomOrderPosition.Create(1));
        }
    }
    
    public ProjectStage CopyToSprint(ProjectSprintAggregate sprint, UserAggregate copiedBy)
    {
        return new ProjectStage(
            ProjectStageId.CreateUnique(),
            Name.Create(Name.Value),
            CustomOrderPosition.Create(CustomOrderPosition.Value), 
            sprint,
            Audit.Create(copiedBy));
    }
}