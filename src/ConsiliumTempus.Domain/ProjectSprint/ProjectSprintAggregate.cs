using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectSprint;

public sealed class ProjectSprintAggregate : Entity<ProjectSprintId>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectSprintAggregate()
    {
    }

    private ProjectSprintAggregate(
        ProjectSprintId id,
        Name name,
        ProjectAggregate project,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        DateOnly? startDate = null,
        DateOnly? endDate = null) : base(id)
    {
        Name = name;
        Project = project;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
        StartDate = startDate;
        EndDate = endDate;
    }

    private readonly List<ProjectStage> _stages = [];

    public Name Name { get; private set; } = default!;
    public DateOnly? StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public ProjectAggregate Project { get; init; } = default!;
    public IReadOnlyList<ProjectStage> Stages => _stages.AsReadOnly();
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }

    public static ProjectSprintAggregate Create(
        Name name,
        ProjectAggregate project,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new ProjectSprintAggregate(
            ProjectSprintId.CreateUnique(),
            name,
            project,
            DateTime.UtcNow,
            DateTime.UtcNow,
            startDate,
            endDate);
    }

    public void Update(
        Name name,
        DateOnly? startDate,
        DateOnly? endDate)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void AddStage(ProjectStage stage, bool onTop = false)
    {
        if (onTop)
        {
            _stages.ForEach(s => 
                s.Update(s.Name, s.CustomOrderPosition + CustomOrderPosition.Create(1)));
            _stages.Insert(0, stage);
        }
        else
        {
            _stages.Add(stage);
        }
    }

    public void RemoveStage(ProjectStage stage)
    {
        _stages.Remove(stage);
        for (var i = stage.CustomOrderPosition.Value; i < _stages.Count; i++)
        {
            var s = _stages[i];
            s.Update(s.Name, s.CustomOrderPosition - CustomOrderPosition.Create(1));
        }
    }
}