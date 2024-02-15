using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Security;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Project.Commands.Create;

public sealed class CreateProjectCommandHandler(
    ISecurity security,
    IWorkspaceRepository workspaceRepository,
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateProjectCommand, ErrorOr<CreateProjectResult>>
{
    public async Task<ErrorOr<CreateProjectResult>> Handle(CreateProjectCommand command, 
        CancellationToken cancellationToken)
    {
        var workspaceId = WorkspaceId.Create(command.WorkspaceId);
        var workspace = await workspaceRepository.Get(workspaceId, cancellationToken);

        if (workspace is null) return Errors.Workspace.NotFound;

        var user = await security.GetUserFromToken(command.Token, cancellationToken);
        
        var project = ProjectAggregate.Create(
            command.Name,
            command.Description,
            command.IsPrivate,
            workspace,
            user);
        await projectRepository.Add(project, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateProjectResult();
    }
}