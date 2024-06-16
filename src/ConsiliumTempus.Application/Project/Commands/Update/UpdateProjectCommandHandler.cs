using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project.Enums;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Update;

public sealed class UpdateProjectCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IProjectRepository projectRepository)
    : IRequestHandler<UpdateProjectCommand, ErrorOr<UpdateProjectResult>>
{
    public async Task<ErrorOr<UpdateProjectResult>> Handle(UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.Get(ProjectId.Create(command.Id), cancellationToken);
        if (project is null) return Errors.Project.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        project.Update(
            Name.Create(command.Name),
            Enum.Parse<ProjectLifecycle>(command.Lifecycle),
            command.IsFavorite,
            user);
        
        return new UpdateProjectResult();
    }
}