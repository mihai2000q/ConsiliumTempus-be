using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Domain.Common.Relations;

public class UserToWorkspace
{
    public UserAggregate User { get; init; } = null!;
    public WorkspaceAggregate Workspace { get; init; } = null!;
    public DateTime CreatedDateTime { get; init; }
    public WorkspaceRole Role { get; init; } = null!;
}