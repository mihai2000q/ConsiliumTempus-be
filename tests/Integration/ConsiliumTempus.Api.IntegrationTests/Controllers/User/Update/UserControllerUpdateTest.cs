using System.Net.Http.Json;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestFactory;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.User.Update;

public class UserControllerUpdateTest(
    WebAppFactory factory,
    ITestOutputHelper testOutputHelper) 
    : BaseIntegrationTest(factory, testOutputHelper)
{
    [Fact]
    public async Task UpdateUser_WhenIsSuccessful_ShouldUpdateAndReturnNewUser()
    {
        // Arrange
        const string email = "michaelj@gmail.com";
        var request = UserRequestFactory.CreateUpdateUserRequest(
            id: new Guid("10000000-0000-0000-0000-000000000000"),
            role: "Software Developer");
        
        // Act
        UseCustomToken(email);
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        var updatedUser = await GetUserById(request.Id);
        Utils.User.AssertUpdate(updatedUser!, request);
        
        await Utils.User.AssertDtoFromResponse(
            outcome,
            request.FirstName, 
            request.LastName, 
            email, 
            request.Id.ToString(),
            request.Role,
            request.DateOfBirth);
    }
    
    [Fact]
    public async Task UpdateUser_WhenUserIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = UserRequestFactory.CreateUpdateUserRequest(
            id: new Guid("90000000-0000-0000-0000-000000000000"));
        
        // Act
        var outcome = await Client.PutAsJsonAsync("api/users", request);

        // Assert
        (await GetUserById(request.Id)).Should().BeNull();
        
        await outcome.ValidateError(Errors.User.NotFound);
    }

    private async Task<UserAggregate?> GetUserById(Guid id)
    {
        var dbContext = await DbContextFactory.CreateDbContextAsync();
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Id == UserId.Create(id));
    }
}