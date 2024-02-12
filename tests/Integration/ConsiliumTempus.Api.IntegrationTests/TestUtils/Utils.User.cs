using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Dto;
using FluentAssertions;

namespace ConsiliumTempus.Api.IntegrationTests.TestUtils;

internal static partial class Utils
{
    internal static class User
    {
        internal static async Task AssertDtoFromResponse(
            HttpResponseMessage response,
            string firstName,
            string lastName,
            string email,
            string id,
            string? role = null,
            DateOnly? dateOfBirth = null)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<UserDto>();
            AssertDto(content!, firstName, lastName, email, id, role, dateOfBirth);
        }
        
        internal static void AssertDto(
            UserDto dto,
            string firstName,
            string lastName,
            string email,
            string id,
            string? role = null,
            DateOnly? dateOfBirth = null)
        {
            dto.Id.Should().Be(id);
            dto.FirstName.Should().Be(firstName);
            dto.LastName.Should().Be(lastName);
            dto.Email.Should().Be(email);
            dto.Role.Should().Be(role);
            dto.DateOfBirth.Should().Be(dateOfBirth);
        }
    }
}