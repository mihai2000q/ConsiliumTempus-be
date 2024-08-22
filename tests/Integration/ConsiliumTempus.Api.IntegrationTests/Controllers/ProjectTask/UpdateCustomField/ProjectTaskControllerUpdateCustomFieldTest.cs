using System.Net.Http.Json;
using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateCustomField;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.Common.Enums;
using ConsiliumTempus.Domain.Common.Errors;
using ConsiliumTempus.Domain.ProjectTask.Entities;
using ConsiliumTempus.Domain.ProjectTask.ValueObjects;
using ConsiliumTempus.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.UpdateCustomField;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerUpdateCustomFieldTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task UpdateCustomFieldFromProjectTask_WhenIsNumberCustomField_ShouldUpdateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks.First();
        var customField = task.CustomFields.First(cf => cf is NumberCustomField);
        var request = ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
            task.Id.Value,
            customField.Id.Value,
            CustomFieldType.Number,
            numberCustomField: new UpdateCustomFieldFromProjectTaskRequest.NumberCustomFieldRequest(10));

        await ActAndAssertSuccess(user, request);
    }

    [Fact]
    public async Task UpdateCustomFieldFromProjectTask_WhenIsSingleSelectCustomField_ShouldUpdateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks.First();
        var customField = task.CustomFields.OfType<SingleSelectCustomField>().First();
        var request = ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
            task.Id.Value,
            customField.Id.Value,
            CustomFieldType.SingleSelect,
            singleSelectCustomField: new UpdateCustomFieldFromProjectTaskRequest.SingleSelectCustomFieldRequest(
                customField.Setup.Options[0].Id));

        await ActAndAssertSuccess(user, request);
    }

    [Fact]
    public async Task UpdateCustomFieldFromProjectTask_WhenIsTextCustomField_ShouldUpdateAndReturnSuccessResponse()
    {
        // Arrange
        var user = ProjectTaskData.Users.First();
        var task = ProjectTaskData.ProjectTasks.First();
        var customField = task.CustomFields.First(cf => cf is TextCustomField);
        var request = ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
            task.Id.Value,
            customField.Id.Value,
            textCustomField: new UpdateCustomFieldFromProjectTaskRequest.TextCustomFieldRequest("New Note"));

        await ActAndAssertSuccess(user, request);
    }

    [Fact]
    public async Task UpdateCustomFieldFromProjectTask_WhenCustomFieldIsNotFound_ShouldReturnCustomFieldNotFoundError()
    {
        // Arrange
        var task = ProjectTaskData.ProjectTasks.First();
        var request = ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
            task.Id.Value,
            textCustomField: new UpdateCustomFieldFromProjectTaskRequest.TextCustomFieldRequest(null));

        // Act
        var outcome = await Client.Put("api/projects/tasks/custom-fields", request);

        // Assert
        await outcome.ValidateError(Errors.CustomField.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.Set<CustomField>().SingleOrDefault(cf => cf.Id == CustomFieldId.Create(request.Id))
            .Should().BeNull();
        dbContext.ProjectTasks.SingleOrDefault(t => t.Id == ProjectTaskId.Create(request.Id))
            .Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateCustomFieldFromProjectTask_WhenIsNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
            Guid.NewGuid(),
            textCustomField: new UpdateCustomFieldFromProjectTaskRequest.TextCustomFieldRequest(null));

        // Act
        var outcome = await Client.Put("api/projects/tasks/custom-fields", request);

        // Assert
        await outcome.ValidateError(Errors.ProjectTask.NotFound);

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        dbContext.ProjectTasks.SingleOrDefault(t => t.Id == ProjectTaskId.Create(request.Id))
            .Should().BeNull();
    }

    private async Task ActAndAssertSuccess(
        UserAggregate user,
        UpdateCustomFieldFromProjectTaskRequest request)
    {
        // Act
        Client.UseCustomToken(user);
        var outcome = await Client.Put("api/projects/tasks/custom-fields", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await outcome.Content.ReadFromJsonAsync<UpdateCustomFieldFromProjectTaskResponse>();
        response!.Message.Should().Be("Custom Field from Project Task has been updated successfully!");

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        var task = await dbContext.ProjectTasks
            .AsNoTracking()
            .Include(t => t.CustomFields)
            .Include(t => t.Stage.Sprint.Project.Workspace)
            .SingleAsync(t => t.Id == ProjectTaskId.Create(request.Id));
        Utils.ProjectTask.AssertUpdateCustomField(task, request);
    }
}