using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
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
        var workspace = await workspaceRepository.GetWithMemberships(
            WorkspaceId.Create(query.Id),
            query.SearchValue,
            cancellationToken);

        var collaborators = workspace?.Memberships
            .Select(m => m.User)
            .ToList();

        return collaborators is not null
            ? new GetCollaboratorsFromWorkspaceResult(collaborators)
            : Errors.Workspace.NotFound;
    }
}