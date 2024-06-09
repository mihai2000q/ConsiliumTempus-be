using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.Workspace.Queries.Get;

public sealed record GetWorkspaceResult(
    WorkspaceAggregate Workspace,
    UserAggregate CurrentUser);