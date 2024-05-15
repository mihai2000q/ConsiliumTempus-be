using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Delete;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Common.UnitTests.Workspace;

public static class WorkspaceResultFactory
{
    public static GetCollectionWorkspaceResult CreateGetCollectionWorkspaceResult(
        List<WorkspaceAggregate>? workspaces = null,
        int totalCount = 25,
        int? totalPages = null)
    {
        return new GetCollectionWorkspaceResult(
            workspaces ?? WorkspaceFactory.CreateList(),
            totalCount,
            totalPages);
    }
    
    public static CreateWorkspaceResult CreateCreateWorkspaceResult()
    {
        return new CreateWorkspaceResult();
    }
    
    public static UpdateWorkspaceResult CreateUpdateWorkspaceResult()
    {
        return new UpdateWorkspaceResult();
    }
    
    public static DeleteWorkspaceResult CreateDeleteWorkspaceResult()
    {
        return new DeleteWorkspaceResult();
    }
}