using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.Project.Entities;

public sealed class ProjectStage : Entity<ProjectStageId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectStage()
    {
    }

    private ProjectStage(
        ProjectStageId id,
        Name name,
        Order order,
        ProjectSprint sprint) : base(id)
    {
        Name = name;
        Order = order;
        Sprint = sprint;
    }

    private readonly List<ProjectTaskAggregate> _tasks = [];

    public Name Name { get; private set; } = default!;
    public Order Order { get; private set; } = default!;
    public ProjectSprint Sprint { get; init; } = default!;
    public IReadOnlyList<ProjectTaskAggregate> Tasks => _tasks.AsReadOnly();

    public static ProjectStage Create(
        Name name,
        Order order,
        ProjectSprint sprint)
    {
        return new ProjectStage(
            ProjectStageId.CreateUnique(),
            name,
            order,
            sprint);
    }

    public void AddTask(ProjectTaskAggregate taskAggregate)
    {
        _tasks.Add(taskAggregate);
    }
}