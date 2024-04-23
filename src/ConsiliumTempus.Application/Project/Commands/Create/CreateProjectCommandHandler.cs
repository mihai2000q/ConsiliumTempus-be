using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Create;

public sealed class CreateProjectCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IWorkspaceRepository workspaceRepository,
    IProjectRepository projectRepository)
    : IRequestHandler<CreateProjectCommand, ErrorOr<CreateProjectResult>>
{
    public async Task<ErrorOr<CreateProjectResult>> Handle(CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var workspaceId = WorkspaceId.Create(command.WorkspaceId);
        var workspace = await workspaceRepository.Get(workspaceId, cancellationToken);
        if (workspace is null) return Errors.Workspace.NotFound;

        var user = await currentUserProvider.GetCurrentUserAfterPermissionCheck(cancellationToken);

        var project = ProjectAggregate.Create(
            Name.Create(command.Name),
            Description.Create(command.Description),
            IsPrivate.Create(command.IsPrivate),
            workspace,
            user);
        await projectRepository.Add(project, cancellationToken);

        workspace.RefreshActivity();

        return new CreateProjectResult();
    }
}