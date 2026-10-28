using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.UpdateStatus;

internal sealed class UpdateStatusFromProjectCommandHandler(
    IProjectRepository projectRepository)
    : IRequestHandler<UpdateStatusFromProjectCommand, ErrorOr<UpdateStatusFromProjectResult>>
{
    public async Task<ErrorOr<UpdateStatusFromProjectResult>> Handle(UpdateStatusFromProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(
            ProjectId.Create(command.Id),
            cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var status = project.Statuses.SingleOrDefault(s => s.Id.Value == command.StatusId);
        if (status is null) return Errors.ProjectStatus.NotFound;

        status.Update(
            Title.Create(command.Title),
            Enum.Parse<ProjectStatusType>(command.Status, true),
            Description.Create(command.Description));

        return new UpdateStatusFromProjectResult();
    }
}