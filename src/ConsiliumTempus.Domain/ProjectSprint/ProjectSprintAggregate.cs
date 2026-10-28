using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;

namespace ConsiliumTempus.Domain.ProjectSprint;

public sealed class ProjectSprintAggregate : AggregateRoot<ProjectSprintId>, IAudit
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectSprintAggregate()
    {
    }

    private ProjectSprintAggregate(
        ProjectSprintId id,
        Name name,
        ProjectAggregate project,
        DateOnly? startDate,
        DateOnly? endDate) : base(id)
    {
        Name = name;
        Project = project;
        StartDate = startDate;
        EndDate = endDate;
    }

    private readonly List<ProjectStage> _stages = [];

    public Name Name { get; private set; } = null!;
    public DateOnly? StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public ProjectAggregate Project { get; init; } = null!;
    public IReadOnlyList<ProjectStage> Stages => _stages.AsReadOnly();
    public Audit Audit { get; init; } = null!;

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
    }

    public void UpdateEndDate(DateOnly? endDate)
    {
        EndDate = endDate;
    }

    public void AddStage(ProjectStage stage, bool onTop = false)
    {
        if (onTop)
        {
            _stages.ForEach(s => s.UpdateCustomOrderPosition(s.CustomOrderPosition + 1));
            _stages.Insert(0, stage);
        }
        else
        {
            _stages.Add(stage);
        }
    }

    public void AddStages(IEnumerable<ProjectStage> stages)
    {
        _stages.AddRange(stages);
    }

    public void RemoveStage(ProjectStage stage)
    {
        for (var i = stage.CustomOrderPosition.Value + 1; i < _stages.Count; i++)
        {
            _stages[i].UpdateCustomOrderPosition(CustomOrderPosition.Create(i - 1));
        }
        _stages.Remove(stage);
    }
}