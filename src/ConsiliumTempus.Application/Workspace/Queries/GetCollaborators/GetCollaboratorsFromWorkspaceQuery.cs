using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.GetCollaborators;

public sealed record GetCollaboratorsFromWorkspaceQuery(
    Guid Id,
    int? CurrentPage,
    int? PageSize,
    string[]? OrderBy,
    string[]? Search,
    string? SearchValue)
    : IRequest<ErrorOr<GetCollaboratorsFromWorkspaceResult>>;