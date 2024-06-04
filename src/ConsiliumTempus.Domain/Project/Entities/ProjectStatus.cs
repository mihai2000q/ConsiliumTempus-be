using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Project.Entities;

public sealed class ProjectStatus : Entity<ProjectStatusId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private ProjectStatus()
    {
    }

    private ProjectStatus(
        ProjectStatusId id,
        Title title,
        ProjectStatusType status,
        Description description,
        ProjectAggregate project,
        Audit audit) : base(id)
    {
        Title = title;
        Status = status;
        Description = description;
        Project = project;
    }

    public Title Title { get; private set; } = default!;
    public ProjectStatusType Status { get; private set; }
    public Description Description { get; private set; } = default!;
    public ProjectAggregate Project { get; init; } = default!;
    public Audit Audit { get; init; } = default!;

    public static ProjectStatus Create(
        Title title,
        ProjectStatusType status,
        Description description,
        ProjectAggregate project,
        UserAggregate createdBy)
    {
        return new ProjectStatus(
            ProjectStatusId.CreateUnique(), 
            title,
            status,
            description,
            project,
            Audit.Create(createdBy));
    }

    public void Update(
        Title title,
        ProjectStatusType status,
        Description description,
        UserAggregate updatedBy)
    {
        Title = title;
        Status = status;
        Description = description;
        Audit.Update(updatedBy);
        Project.RefreshActivity();
    }
}