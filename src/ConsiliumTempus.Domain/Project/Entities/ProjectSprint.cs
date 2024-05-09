using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Domain.Project.Entities;

public sealed class ProjectSprint : Entity<ProjectSprintId>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectSprint()
    {
    }

    private ProjectSprint(
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

    public static ProjectSprint Create(
        Name name,
        ProjectAggregate project,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new ProjectSprint(
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

    public void AddStage(ProjectStage stage)
    {
        _stages.Add(stage);
    }
    
    public void RemoveStage(ProjectStage stage)
    {
        if (!_stages.Remove(stage)) return;
        for (var i = stage.CustomOrderPosition.Value; i < stage.Sprint.Stages.Count; i++)
        {
            stage.Sprint.Stages[i].DecrementCustomOrderPosition();
        }
    }
}