using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Announcement;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Announcement.Commands.Create;

public sealed class CreateAnnouncementCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository,
    IAnnouncementRepository announcementRepository)
    : IRequestHandler<CreateAnnouncementCommand, ErrorOr<CreateAnnouncementResult>>
{
    public async Task<ErrorOr<CreateAnnouncementResult>> Handle(CreateAnnouncementCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(ProjectId.Create(command.ProjectId), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        var announcement = AnnouncementAggregate.Create(
            Title.Create(command.Title),
            Description.Create(command.Description),
            user,
            project);
        await announcementRepository.Add(announcement, cancellationToken);

        return new CreateAnnouncementResult();
    }
}