using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.Workspace;

namespace ConsiliumTempus.Application.UnitTests.Mock;

internal static partial class Mock
{
    internal static class Project
    {
        internal static ProjectAggregate CreateMock(
            WorkspaceAggregate? workspace = null, 
            UserAggregate? user = null)
        {
            workspace ??= Workspace.CreateMock();
            user ??= User.CreateMock();
            
            return ProjectAggregate.Create(
                "Project Name",
                "This is the project description",
                true,
                workspace,
                user);
        }
    }
}