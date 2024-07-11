using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.ProjectSprint.Entities;
using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.ProjectSprint;

public sealed class ProjectSprintAggregate : Entity<ProjectSprintId>
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
        DateOnly? endDate,
        Audit audit) : base(id)
    {
        Name = name;
        Project = project;
        StartDate = startDate;
        EndDate = endDate;
        Audit = audit;
    }

    private readonly List<ProjectStage> _stages = [];

    public Name Name { get; private set; } = default!;
    public DateOnly? StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public ProjectAggregate Project { get; init; } = default!;
    public IReadOnlyList<ProjectStage> Stages => _stages.AsReadOnly();
    public Audit Audit { get; } = default!;

    public static ProjectSprintAggregate Create(
        Name name,
        ProjectAggregate project,
        UserAggregate createdBy,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new ProjectSprintAggregate(
            ProjectSprintId.CreateUnique(),
            name,
            project,
            startDate,
            endDate,
            Audit.Create(createdBy));
    }

    public void Update(
        Name name,
        DateOnly? startDate,
        DateOnly? endDate,
        UserAggregate updatedBy)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Audit.Update(updatedBy);
    }
    
    public void UpdateEndDate(DateOnly? endDate, UserAggregate updatedBy)
    {
        EndDate = endDate;
        Audit.Update(updatedBy);
    }

    public void AddStage(ProjectStage stage, bool onTop = false)
    {
        if (onTop)
        {
            _stages.ForEach(s =>
                s.UpdateWithoutAudit(s.Name, s.CustomOrderPosition + 1));
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
        _stages.Remove(stage);
        for (var i = stage.CustomOrderPosition.Value; i < _stages.Count; i++)
        {
            var s = _stages[i];
            s.UpdateWithoutAudit(s.Name, s.CustomOrderPosition - 1);
        }
    }
}