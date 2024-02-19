using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.ProjectTask;

namespace ConsiliumTempus.Domain.Project.Entities;

public sealed class ProjectSection : Entity<ProjectSectionId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectSection()
    {
    }

    private ProjectSection(
        ProjectSectionId id,
        string name,
        int order,
        ProjectSprint sprint) : base(id)
    {
        Name = name;
        Order = order;
        Sprint = sprint;
    }

    private readonly List<ProjectTaskAggregate> _tasks = [];

    public string Name { get; private set; } = string.Empty;
    public int Order { get; private set; }
    public ProjectSprint Sprint { get; init; } = default!;
    public IReadOnlyList<ProjectTaskAggregate> Tasks => _tasks.AsReadOnly();

    public static ProjectSection Create(
        string name,
        int order,
        ProjectSprint sprint)
    {
        return new ProjectSection(
            ProjectSectionId.CreateUnique(), 
            name, 
            order,
            sprint);
    }

    public void AddTask(ProjectTaskAggregate taskAggregate)
    {
        _tasks.Add(taskAggregate);
    }
}