using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;

public sealed record GetCollaboratorsFromWorkspaceQuery(
    Guid Id,
    string SearchValue)
    : IRequest<ErrorOr<GetCollaboratorsFromWorkspaceResult>>;