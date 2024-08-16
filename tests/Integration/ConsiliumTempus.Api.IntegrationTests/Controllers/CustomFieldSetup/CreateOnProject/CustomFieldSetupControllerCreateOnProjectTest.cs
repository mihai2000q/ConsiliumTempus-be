using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnProject;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Project;
using ConsiliumTempus.Domain.Project.ValueObjects;
using ConsiliumTempus.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.CreateOnProject;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerCreateOnProjectTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task
        CreateCustomFieldSetupOnProject_WhenRequestHasNumberType_ShouldCreateNumberCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var project = CustomFieldSetupData.Projects.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
            project.Id.Value,
            type: CustomFieldType.Number,
            numberCustomFieldSetup: new CreateCustomFieldSetupOnProjectRequest.CreateNumberCustomFieldSetupRequest(
                new CreateCustomFieldSetupOnProjectRequest.CreateNumberCustomFieldSetupRequest.NumberSettingsRequest(
                    "USD",
                    2,
                    false),
                null));

        await ActAndAssert(request, user, project);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnProject_WhenRequestHasSingleSelectType_ShouldCreateSingleSelectCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var project = CustomFieldSetupData.Projects.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
            project.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new
                CreateCustomFieldSetupOnProjectRequest.CreateSingleSelectCustomFieldSetupRequest(
                    [
                        new CreateCustomFieldSetupOnProjectRequest.CreateSingleSelectCustomFieldSetupRequest.
                            SingleSelectOptionRequest(
                                "1",
                                "High",
                                "#FF2233"),
                        new CreateCustomFieldSetupOnProjectRequest.CreateSingleSelectCustomFieldSetupRequest.
                            SingleSelectOptionRequest(
                                "2",
                                "Low",
                                "#7788AA")
                    ],
                    null));

        await ActAndAssert(request, user, project);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnProject_WhenRequestHasTextType_ShouldCreateTextCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var project = CustomFieldSetupData.Projects.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(
            project.Id.Value,
            textCustomFieldSetup: new CreateCustomFieldSetupOnProjectRequest.CreateTextCustomFieldSetupRequest(null));

        await ActAndAssert(request, user, project);
    }

    [Fact]
    public async Task CreateCustomFieldSetupOnProject_WhenProjectIsNotFound_ShouldReturnProjectNotFoundError()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnProjectRequest(Guid.NewGuid());

        // Act
        var outcome = await Client.Post("api/customFieldSetups/project", request);

        // Assert
        await outcome.ValidateError(Errors.Project.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.Should().HaveCount(CustomFieldSetupData.CustomFieldSetups.Length);
        dbContext.CustomFieldSetups.SingleOrDefault(p => p.Name == Name.Create(request.Name))
            .Should().BeNull();
        dbContext.Projects.SingleOrDefault(p => p.Id == ProjectId.Create(request.ProjectId!.Value))
            .Should().BeNull();
    }

    private async Task ActAndAssert(
        CreateCustomFieldSetupOnProjectRequest request,
        UserAggregate user,
        ProjectAggregate project)
    {
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/customFieldSetups/project", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateCustomFieldSetupOnProjectResponse>();
        response!.Message.Should().Be("Custom Field Setup has been created successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.Should().HaveCount(CustomFieldSetupData.CustomFieldSetups.Length + 1);
        var createdCustomFieldSetup = await dbContext.CustomFieldSetups
            .AsNoTracking()
            .Include(cfs => cfs.Audit)
            .Include(cfs => cfs.Workspace)
            .Include(cfs => cfs.Project)
            .SingleAsync(ps => ps.Name == Name.Create(request.Name));

        var tasks = await dbContext.ProjectTasks
            .AsNoTracking()
            .Include(t => t.CustomFields)
            .Where(t => t.Stage.Sprint.Project == project)
            .ToListAsync();

        Utils.CustomFieldSetup.AssertCreateOnProject(
            request,
            createdCustomFieldSetup,
            user,
            project,
            tasks);
    }
}