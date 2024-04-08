using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForWorkspace;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Project
    {
        internal static void AssertGetCollectionForUserResponse(
            GetCollectionProjectForUserResponse response,
            IEnumerable<ProjectAggregate> projects)
        {
            response.Projects
                .Zip(projects)
                .Should().AllSatisfy(p => AssertResponse(p.First, p.Second));
        }
        
        internal static void AssertGetCollectionForWorkspaceResponse(
            GetCollectionProjectForWorkspaceResponse response,
            IEnumerable<ProjectAggregate> projects)
        {
            response.Projects
                .Zip(projects)
                .Should().AllSatisfy(p => AssertResponse(p.First, p.Second));
        }
        
        internal static void AssertCreation(ProjectAggregate project, CreateProjectRequest request)
        {
            project.Name.Value.Should().Be(request.Name);
            project.Description.Value.Should().Be(request.Description);
            project.IsPrivate.Value.Should().Be(request.IsPrivate);
            project.Workspace.Id.Value.Should().Be(request.WorkspaceId);

            project.Sprints.Should().HaveCount(1);
            project.Sprints[0].Sections.Should().HaveCount(Constants.ProjectSection.Names.Length);
            project.Sprints[0].Sections[0].Tasks
                .Should().HaveCount(Constants.ProjectTask.Names.Length);
        }
        
        private static void AssertResponse(
            GetCollectionProjectForUserResponse.ProjectResponse projectResponse,
            ProjectAggregate project)
        {
            projectResponse.Id.Should().Be(project.Id.Value);
            projectResponse.Name.Should().Be(project.Name.Value);
        }
        
        private static void AssertResponse(
            GetCollectionProjectForWorkspaceResponse.ProjectResponse projectResponse,
            ProjectAggregate project)
        {
            projectResponse.Id.Should().Be(project.Id.Value);
            projectResponse.Name.Should().Be(project.Name.Value);
            projectResponse.Description.Should().Be(project.Description.Value);
        }
    }
}