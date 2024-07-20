using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Announcement.ValueObjects;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Announcement;

public sealed class AnnouncementAggregate : AggregateRoot<AnnouncementId, Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private AnnouncementAggregate()
    {
    }

    private AnnouncementAggregate(
        AnnouncementId id,
        Title title,
        Description description,
        Audit audit,
        ProjectAggregate? project,
        WorkspaceAggregate? workspace) : base(id)
    {
        Title = title;
        Description = description;
        Audit = audit;
        Project = project;
        Workspace = workspace;
    }

    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public Audit Audit { get; init; } = default!;
    public ProjectAggregate? Project { get; init; }
    public WorkspaceAggregate? Workspace { get; init; }

    public static AnnouncementAggregate Create(
        Title title,
        Description description,
        UserAggregate createdBy,
        ProjectAggregate project)
    {
        return new AnnouncementAggregate(
            AnnouncementId.CreateUnique(),
            title,
            description,
            Audit.Create(createdBy),
            project,
            null);
    }
}