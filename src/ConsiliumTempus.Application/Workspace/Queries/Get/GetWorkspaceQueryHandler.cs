using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.Get;

public sealed class GetWorkspaceQueryHandler(IWorkspaceRepository workspaceRepository)
    : IRequestHandler<GetWorkspaceQuery, ErrorOr<GetWorkspaceResult>>
{
    public async Task<ErrorOr<GetWorkspaceResult>> Handle(GetWorkspaceQuery query, CancellationToken cancellationToken)
    {
        var workspaceId = WorkspaceId.Create(query.Id);
        var workspace = await workspaceRepository.Get(workspaceId, cancellationToken);

        if (workspace is null) return Errors.Workspace.NotFound;

        return new GetWorkspaceResult(workspace);
    }
}