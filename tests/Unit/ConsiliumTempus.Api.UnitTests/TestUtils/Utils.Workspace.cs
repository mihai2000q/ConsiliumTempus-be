using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Contracts.Workspace.Get;
using ConsiliumTempus.Api.Contracts.Workspace.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.Workspace.Commands.Create;
using ConsiliumTempus.Application.Workspace.Commands.Update;
using ConsiliumTempus.Application.Workspace.Queries.Get;
using ConsiliumTempus.Application.Workspace.Queries.GetCollection;
using ConsiliumTempus.Domain.Workspace;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliumTempus.Api.UnitTests.TestUtils;

internal static partial class Utils
{
    internal static class Workspace
    {
        internal static bool AssertGetQuery(
            GetWorkspaceQuery query,
            GetWorkspaceRequest request)
        {
            query.Id.Should().Be(request.Id);
            return true;
        }

        internal static bool AssertGetCollectionQuery(
            GetCollectionWorkspaceQuery query,
            string token)
        {
            query.Token.Should().Be(token);
            return true;
        }

        internal static bool AssertCreateCommand(
            CreateWorkspaceCommand command,
            CreateWorkspaceRequest request,
            string token)
        {
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            command.Token.Should().Be(token);
            return true;
        }

        internal static bool AssertUpdateCommand(
            UpdateWorkspaceCommand command,
            UpdateWorkspaceRequest request)
        {
            command.Id.Should().Be(request.Id);
            command.Name.Should().Be(request.Name);
            command.Description.Should().Be(request.Description);
            return true;
        }

        internal static void AssertDto(
            IActionResult outcome,
            WorkspaceAggregate workspace)
        {
            outcome.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)outcome).Value.Should().BeOfType<WorkspaceDto>();

            var response = ((OkObjectResult)outcome).Value as WorkspaceDto;

            AssertDto(response!, workspace);
        }

        internal static void AssertDtos(
            IActionResult outcome,
            IEnumerable<WorkspaceAggregate> workspaces)
        {
            outcome.Should().BeOfType<OkObjectResult>();

            var response = ((OkObjectResult)outcome).Value as IEnumerable<WorkspaceDto>;

            response!.Zip(workspaces)
                .ToList()
                .ForEach(x => AssertDto(x.First, x.Second));
        }

        private static void AssertDto(WorkspaceDto dto, WorkspaceAggregate workspace)
        {
            dto.Id.Should().Be(workspace.Id.Value.ToString());
            dto.Name.Should().Be(workspace.Name.Value);
            dto.Description.Should().Be(workspace.Description.Value);
        }
    }
}