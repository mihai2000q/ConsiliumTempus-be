using System.Net;
using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.Authentication.Register;
using ConsiliumTempus.Api.Contracts.User.Update;
using ConsiliumTempus.Api.Dto;
using ConsiliumTempus.Application.Common.Extensions;
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
            user.Name.First.Should().Be(request.FirstName.Capitalize());
            user.Name.Last.Should().Be(request.LastName.Capitalize());
            user.Credentials.Email.Should().Be(request.Email.ToLower());
            user.Credentials.Password.Should().NotBeNullOrWhiteSpace().And.NotBe(request.Password);
            user.Role.Should().Be(request.Role);
            user.DateOfBirth.Should().Be(request.DateOfBirth);
        }
        
        internal static void AssertUpdate(
            UserAggregate user,
            UpdateUserRequest request)
        {
            user.Id.Value.Should().Be(request.Id);
            user.Name.First.Should().Be(request.FirstName.Capitalize());
            user.Name.Last.Should().Be(request.LastName.Capitalize());
            user.Role.Should().Be(request.Role);
            user.DateOfBirth.Should().Be(request.DateOfBirth);
        }
        
        internal static void AssertNotUpdated(
            UserAggregate user,
            UpdateUserRequest request)
        {
            user.Id.Value.Should().Be(request.Id);
            user.Name.First.Should().NotBe(request.FirstName.Capitalize());
            user.Name.Last.Should().NotBe(request.LastName.Capitalize());
            user.Role.Should().NotBe(request.Role);
            user.DateOfBirth.Should().BeNull();
        }
    }
}