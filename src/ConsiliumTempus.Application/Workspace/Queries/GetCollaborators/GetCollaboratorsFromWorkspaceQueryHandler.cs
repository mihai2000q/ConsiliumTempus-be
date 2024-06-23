using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;

public sealed class GetCollaboratorsFromWorkspaceQueryHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetCollaboratorsFromWorkspaceQuery, ErrorOr<GetCollaboratorsFromWorkspaceResult>>
{
    public async Task<ErrorOr<GetCollaboratorsFromWorkspaceResult>> Handle(GetCollaboratorsFromWorkspaceQuery query,
        CancellationToken cancellationToken)
    {
        var collaborators = await workspaceRepository.GetCollaborators(
            WorkspaceId.Create(query.Id),
            query.SearchValue?.Trim(),
            cancellationToken);

        return new GetCollaboratorsFromWorkspaceResult(collaborators);
    }
}