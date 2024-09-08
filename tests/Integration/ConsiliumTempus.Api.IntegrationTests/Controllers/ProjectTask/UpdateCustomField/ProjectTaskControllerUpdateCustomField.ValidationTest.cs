using ConsiliumTempus.Api.Contracts.ProjectTask.UpdateCustomField;
using ConsiliumTempus.Api.IntegrationTests.Core;
using ConsiliumTempus.Api.IntegrationTests.TestCollections;
using ConsiliumTempus.Api.IntegrationTests.TestData;
using ConsiliumTempus.Api.IntegrationTests.TestUtils;
using ConsiliumTempus.Common.IntegrationTests.ProjectTask;
using ConsiliumTempus.Domain.ProjectTask.Entities;

namespace ConsiliumTempus.Api.IntegrationTests.Controllers.ProjectTask.UpdateCustomField;

[Collection(nameof(ProjectTaskControllerCollection))]
public class ProjectTaskControllerUpdateCustomFieldValidationTest(WebAppFactory factory)
    : BaseIntegrationTest(factory, new ProjectTaskData())
{
    [Fact]
    public async Task UpdateCustomFieldFromProjectTask_WhenRequestIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var task = ProjectTaskData.ProjectTasks.First();
        var customField = task.CustomFields.First(cf => cf is TextCustomField);
        var request = ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
            task.Id.Value,
            customField.Id.Value,
            textCustomField: new UpdateCustomFieldFromProjectTaskRequest.TextCustomFieldRequest("something"));

        // Act
        Client.UseCustomToken(ProjectTaskData.Users.First());
        var outcome = await Client.Put("api/projects/tasks/custom-fields", request);

        // Assert
        outcome.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateCustomFieldFromProjectTask_WhenRequestIsInvalid_ShouldReturnValidationErrors()
    {
        // Arrange
        var request = ProjectTaskRequestFactory.CreateUpdateCustomFieldFromProjectTaskRequest(
            id: Guid.Empty,
            customFieldId: Guid.Empty);  

        // Act
        var outcome = await Client.Put("api/projects/tasks/custom-fields", request);

        // Assert
        await outcome.ValidateValidationErrors();
    }
}