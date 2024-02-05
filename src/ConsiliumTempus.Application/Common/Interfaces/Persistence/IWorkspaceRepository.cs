using ConsiliumTempus.Domain.WorkspaceAggregate;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence;

public interface IWorkspaceRepository
{
    Task Add(Workspace workspace);
}