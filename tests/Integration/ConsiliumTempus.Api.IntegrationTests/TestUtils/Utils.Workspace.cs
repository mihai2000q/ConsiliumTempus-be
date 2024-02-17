using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Workspace.Create;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Domain.Workspace;
using FluentAssertions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class Workspace
    {
        internal static async Task AssertDtoFromResponse(
            HttpResponseMessage response,
            string name,
            string description,
            string? id = null)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<WorkspaceDto>();
            AssertDto(content!, name, description, id);
        }

        internal static void AssertDto(
            WorkspaceDto dto,
            string name,
            string description,
            string? id = null)
        {
            if (id is null)
            {
                dto.Id.Should().NotBeNullOrWhiteSpace();
            }
            else
            {
                dto.Id.Should().Be(id);
            }
            dto.Name.Should().Be(name);
            dto.Description.Should().Be(description);
        }

        internal static void AssertCreation(WorkspaceAggregate workspace, CreateWorkspaceRequest request)
        {
            workspace.Id.Value.Should().NotBeEmpty();
            workspace.Name.Should().Be(request.Name);
            workspace.Description.Should().Be(request.Description);
        }
    }
}