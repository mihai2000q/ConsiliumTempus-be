using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Constants;
using ConsiliumTempus.Domain.User;
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

        internal static void AssertRegistration(
            UserAggregate user,
            RegisterRequest request)
        {
            user.Id.Value.Should().NotBeEmpty();
            user.FirstName.Value.Should().Be(request.FirstName.Capitalize());
            user.LastName.Value.Should().Be(request.LastName.Capitalize());
            user.Credentials.Email.Should().Be(request.Email.ToLower());
            user.Credentials.Password.Should().NotBeNullOrWhiteSpace().And.NotBe(request.Password);
            if (request.Role is null)
                user.Role.Should().BeNull();
            else
                user.Role!.Value.Should().Be(request.Role);
            user.DateOfBirth.Should().Be(request.DateOfBirth);

            user.Memberships.Should().HaveCount(1);
            user.Memberships[0].User.Should().Be(user);
            user.Memberships[0].Workspace.Name.Value.Should().Be(Constants.Workspace.Name);
            user.Memberships[0].Workspace.Description.Value.Should().Be(Constants.Workspace.Description);
            user.Memberships[0].Workspace.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        internal static void AssertUpdate(
            UserAggregate user,
            UpdateUserRequest request)
        {
            user.Id.Value.Should().Be(request.Id);
            user.FirstName.Value.Should().Be(request.FirstName.Capitalize());
            user.LastName.Value.Should().Be(request.LastName.Capitalize());
            if (request.Role is null)
                user.Role.Should().BeNull();
            else
                user.Role!.Value.Should().Be(request.Role);
            user.DateOfBirth.Should().Be(request.DateOfBirth);
        }

        internal static void AssertNotUpdated(
            UserAggregate user,
            UpdateUserRequest request)
        {
            user.Id.Value.Should().Be(request.Id);
            user.FirstName.Value.Should().NotBe(request.FirstName.Capitalize());
            user.LastName.Value.Should().NotBe(request.LastName.Capitalize());
            user.Role?.Value.Should().NotBe(request.Role);
            user.DateOfBirth.Should().NotBe(request.DateOfBirth);
        }
    }
}