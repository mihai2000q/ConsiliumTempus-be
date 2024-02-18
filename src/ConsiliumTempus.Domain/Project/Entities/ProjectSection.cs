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
        ProjectSprint sprint) : base(id)
    {
        Name = name;
        Sprint = sprint;
    }

    private readonly List<ProjectTaskAggregate> _tasks = [];

    public string Name { get; private set; } = string.Empty;
    public ProjectSprint Sprint { get; init; } = default!;
    public IReadOnlyList<ProjectTaskAggregate> Tasks => _tasks.AsReadOnly();

    public static ProjectSection Create(
        string name,
        ProjectSprint sprint)
    {
        return new ProjectSection(
            ProjectSectionId.CreateUnique(), 
            name, 
            sprint);
    }

    public void AddTask(ProjectTaskAggregate taskAggregate)
    {
        _tasks.Add(taskAggregate);
    }
}