using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IWorkspaceRoleRepository
{
    void Attach(WorkspaceRole workspaceRole);
}