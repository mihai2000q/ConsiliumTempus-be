using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Project.ValueObjects;

namespace ConsiliumTempus.Domain.Project.Entities;

public sealed class ProjectSection : Entity<ProjectSectionId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private ProjectSection()
    {
    }

    private ProjectSection(
        string name,
        ProjectSprint sprint)
    {
        Name = name;
        Sprint = sprint;
    }

    private readonly List<ProjectTask> _tasks = [];

    public string Name { get; private set; } = string.Empty;
    public ProjectSprint Sprint { get; init; } = default!;
    public IReadOnlyList<ProjectTask> Tasks => _tasks.AsReadOnly();

    public static ProjectSection Create(
        string name,
        ProjectSprint sprint)
    {
        return new ProjectSection(name, sprint);
    }

    public void AddTask(ProjectTask task)
    {
        _tasks.Add(task);
    }
}