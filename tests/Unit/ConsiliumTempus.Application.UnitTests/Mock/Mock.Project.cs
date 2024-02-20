using ConsiliumTempus.Domain.Common.ValueObjects;
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
                Name.Create("Project Name"),
                Description.Create("This is the project description"),
                IsPrivate.Create(true),
                workspace,
                user);
        }
    }
    
    internal static class ProjectSprint
    {
        internal static Domain.Project.Entities.ProjectSprint CreateMock(ProjectAggregate? project = null)
        {
            project ??= Project.CreateMock();
            
            return Domain.Project.Entities.ProjectSprint.Create(
                Name.Create("Project Sprint Name"),
                project);
        }
    }
}