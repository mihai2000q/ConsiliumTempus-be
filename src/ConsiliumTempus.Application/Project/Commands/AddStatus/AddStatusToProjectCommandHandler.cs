using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Entities;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.AddStatus;

internal sealed class AddStatusToProjectCommandHandler(
    IProjectRepository projectRepository)
    : IRequestHandler<AddStatusToProjectCommand, ErrorOr<AddStatusToProjectResult>>
{
    public async Task<ErrorOr<AddStatusToProjectResult>> Handle(AddStatusToProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(
            ProjectId.Create(command.Id),
            cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        project.AddStatus(ProjectStatus.Create(
            Title.Create(command.Title),
            command.Status,
            Description.Create(command.Description),
            project));

        project.RefreshActivity();

        return new AddStatusToProjectResult();
    }
}