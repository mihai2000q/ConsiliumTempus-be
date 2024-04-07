using ConsiliumTempus.Api.Contracts.Project.Create;
using ConsiliumTempus.Api.Contracts.Project.Entities.Sprint.Create;
using ConsiliumTempus.Api.Contracts.Project.GetCollectionForUser;
using ConsiliumTempus.Application.Project.Commands.Create;
using ConsiliumTempus.Application.Project.Commands.Delete;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Create;
using ConsiliumTempus.Application.Project.Entities.Sprint.Commands.Delete;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Project
    {
        internal static void AssertProjectResponse(
            GetCollectionProjectForUserResponse.ProjectResponse projectResponse, 
            ProjectAggregate project)
        {
            projectResponse.Id.Should().Be(project.Id.Value);
            projectResponse.Name.Should().Be(project.Name.Value);
        }
        
        internal static bool AssertCreateCommand(CreateProjectCommand command, CreateProjectRequest request)
        {
            command.WorkspaceId.Should().Be(request.WorkspaceId);
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.IsPrivate.Should().Be(request.IsPrivate);
            return true;
        }

        internal static bool AssertDeleteCommand(DeleteProjectCommand command, Guid id)
        {
            command.Id.Should().Be(id);
            return true;
        }
    }
    
    internal static class ProjectSprint
    {
        internal static bool AssertCreateCommand(CreateProjectSprintCommand command, CreateProjectSprintRequest request)
        {
            command.ProjectId.Should().Be(request.ProjectId);
            command.Name.Should().Be(request.Name);
            command.StartDate.Should().Be(request.StartDate);
            command.EndDate.Should().Be(request.EndDate);
            return true;
        }

        internal static bool AssertDeleteCommand(DeleteProjectSprintCommand command, Guid id)
        {
            command.Id.Should().Be(id);
            return true;
        }
    }
}