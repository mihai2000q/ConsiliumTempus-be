using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Update;

public sealed class UpdateProjectCommandHandler(IProjectRepository projectRepository)
    : IRequestHandler<UpdateProjectCommand, ErrorOr<UpdateProjectResult>>
{
    public async Task<ErrorOr<UpdateProjectResult>> Handle(UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(ProjectId.Create(command.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        project.Update(
            Name.Create(command.Name),
            Enum.Parse<ProjectLifecycle>(command.Lifecycle));

        return new UpdateProjectResult();
    }
}