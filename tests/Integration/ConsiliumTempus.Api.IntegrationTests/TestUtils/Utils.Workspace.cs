using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Dto;
using FluentAssertions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

public static partial class Utils
{
    public static class Workspace
    {
        public static async Task AssertDtoFromResponse(
            HttpResponseMessage response,
            string name,
            string description)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<WorkspaceDto>();
            content!.Id.Should().NotBeNullOrWhiteSpace();
            content.Name.Should().Be(name);
            content.Description.Should().Be(description);
        }
    }
}