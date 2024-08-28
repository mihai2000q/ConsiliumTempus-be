using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.Create;
using ConsiliumTempus.Api.Contracts.CustomFieldSetup.CreateOnWorkspace;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.CustomFieldSetup;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.Workspace.ValueObjects;
using ConsiliumTempus.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.CustomFieldSetup.CreateOnWorkspace;

[Collection(nameof(CustomFieldSetupControllerCollection))]
public class CustomFieldSetupControllerCreateOnWorkspaceTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new CustomFieldSetupData())
{
    [Fact]
    public async Task
        CreateCustomFieldSetupOnWorkspace_WhenRequestHasDateType_ShouldCreateDateCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            workspace.Id.Value,
            type: CustomFieldType.Date,
            dateCustomFieldSetup: new CreateCustomFieldSetupRequest.DateCustomFieldSetupRequest(
                new DateOnly(2021, 12, 1)));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnWorkspace_WhenRequestHasDateTimeType_ShouldCreateDateTimeCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            workspace.Id.Value,
            type: CustomFieldType.DateTime,
            dateTimeCustomFieldSetup: new CreateCustomFieldSetupRequest.DateTimeCustomFieldSetupRequest(
                new DateTime(2022, 12, 1, 10, 55, 30)));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnWorkspace_WhenRequestHasDurationType_ShouldCreateDurationCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            workspace.Id.Value,
            type: CustomFieldType.Duration,
            durationCustomFieldSetup: new CreateCustomFieldSetupRequest.DurationCustomFieldSetupRequest(
                new TimeSpan(5, 10, 5)));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnWorkspace_WhenRequestHasMultiSelectType_ShouldCreateMultiSelectCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            workspace.Id.Value,
            type: CustomFieldType.MultiSelect,
            multiSelectCustomFieldSetup: new
                CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest(
                [
                    new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.
                        MultiSelectOptionRequest(
                            "Higher",
                            "#FF2233"),
                    new CreateCustomFieldSetupRequest.MultiSelectCustomFieldSetupRequest.
                        MultiSelectOptionRequest(
                            "Lower",
                            "#7788AA")
                ]));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnWorkspace_WhenRequestHasNumberType_ShouldCreateNumberCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            workspace.Id.Value,
            type: CustomFieldType.Number,
            numberCustomFieldSetup: new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest(
                new CreateCustomFieldSetupRequest.NumberCustomFieldSetupRequest.NumberSettingsRequest(
                    "USD",
                    2,
                    false),
                12));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnWorkspace_WhenRequestHasPeopleType_ShouldCreatePeopleCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            workspace.Id.Value,
            type: CustomFieldType.People);

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnWorkspace_WhenRequestHasSingleSelectType_ShouldCreateSingleSelectCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            workspace.Id.Value,
            type: CustomFieldType.SingleSelect,
            singleSelectCustomFieldSetup: new
                CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest(
                    [
                        new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.
                            SingleSelectOptionRequest(
                                "1",
                                "High",
                                "#FF2233"),
                        new CreateCustomFieldSetupRequest.SingleSelectCustomFieldSetupRequest.
                            SingleSelectOptionRequest(
                                "2",
                                "Low",
                                "#7788AA")
                    ],
                    "2"));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnWorkspace_WhenRequestHasTextType_ShouldCreateTextCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            workspace.Id.Value,
            textCustomFieldSetup: new CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest("Default"));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task
        CreateCustomFieldSetupOnWorkspace_WhenRequestHasTimeType_ShouldCreateTimeCustomFieldSetupAndReturnSuccessResponse()
    {
        // Arrange
        var user = CustomFieldSetupData.Users.First();
        var workspace = CustomFieldSetupData.Workspaces.First();
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            workspace.Id.Value,
            type: CustomFieldType.Time,
            timeCustomFieldSetup: new CreateCustomFieldSetupRequest.TimeCustomFieldSetupRequest(
                new TimeOnly(10, 55)));

        await ActAndAssert(request, user);
    }

    [Fact]
    public async Task CreateCustomFieldSetupOnWorkspace_WhenWorkspaceIsNotFound_ShouldReturnWorkspaceNotFoundError()
    {
        // Arrange
        var request = CustomFieldSetupRequestFactory.CreateCreateCustomFieldSetupOnWorkspaceRequest(
            Guid.NewGuid(),
            textCustomFieldSetup: new CreateCustomFieldSetupRequest.TextCustomFieldSetupRequest(null));

        // Act
        var outcome = await Client.Post("api/customFieldSetups/Workspace", request);

        // Assert
        await outcome.ValidateError(Errors.Workspace.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.Should().HaveCount(CustomFieldSetupData.CustomFieldSetups.Length);
        dbContext.CustomFieldSetups.SingleOrDefault(p => p.Name == Name.Create(request.Name))
            .Should().BeNull();
        dbContext.Workspaces.SingleOrDefault(p => p.Id == WorkspaceId.Create(request.WorkspaceId))
            .Should().BeNull();
    }

    private async Task ActAndAssert(CreateCustomFieldSetupOnWorkspaceRequest request, UserAggregate user)
    {
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Post("api/customFieldSetups/Workspace", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<CreateCustomFieldSetupOnWorkspaceResponse>();
        response!.Message.Should().Be("Custom Field Setup has been created successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.CustomFieldSetups.Should().HaveCount(CustomFieldSetupData.CustomFieldSetups.Length + 1);
        var createdCustomFieldSetup = await dbContext.CustomFieldSetups
            .AsNoTracking()
            .Include(cfs => cfs.Audit)
            .Include(cfs => cfs.Workspace)
            .Include(cfs => cfs.Projects)
            .SingleAsync(cfs => cfs.Name == Name.Create(request.Name));

        Utils.CustomFieldSetup.AssertCreation(
            request,
            createdCustomFieldSetup,
            user,
            []);
    }
}