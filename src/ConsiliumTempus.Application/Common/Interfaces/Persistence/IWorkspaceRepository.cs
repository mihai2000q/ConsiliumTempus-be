using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence;

public interface IWorkspaceRepository
{
    Task Add(WorkspaceAggregate workspace);
}