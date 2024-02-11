using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Workspace.Queries.Get;

public sealed record GetWorkspaceQuery(string Id) 
    : IRequest<ErrorOr<GetWorkspaceResult>>;