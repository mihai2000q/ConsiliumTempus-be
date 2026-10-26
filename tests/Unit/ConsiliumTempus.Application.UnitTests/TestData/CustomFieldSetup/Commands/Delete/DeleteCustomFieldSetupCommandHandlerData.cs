using ConsiliumTempus.Common.UnitTests.CustomFieldSetup;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.Workspace;
using ConsiliumTempus.Domain.CustomFieldSetup;

namespace ConsiliumTempus.Application.UnitTests.TestData.CustomFieldSetup.Commands.Delete;

internal static class DeleteCustomFieldSetupCommandHandlerData
{
    internal class GetCustomFieldSetups : TheoryData<CustomFieldSetupAggregate>
    {
        public GetCustomFieldSetups()
        {
            // Global, without projects
            Add(CustomFieldSetupFactory.CreateText());

            // Workspace, without projects
            Add(CustomFieldSetupFactory.CreateText(workspace: WorkspaceFactory.Create()));

            // Global, with projects
            var customFieldSetup = CustomFieldSetupFactory.CreateText();
            customFieldSetup.AddProject(ProjectFactory.Create());
            customFieldSetup.AddProject(ProjectFactory.Create());
            customFieldSetup.ClearDomainEvents();
            Add(customFieldSetup);

            // Workspace, with projects
            customFieldSetup = CustomFieldSetupFactory.CreateText(workspace: WorkspaceFactory.Create());
            customFieldSetup.AddProject(ProjectFactory.Create());
            customFieldSetup.AddProject(ProjectFactory.Create());
            customFieldSetup.ClearDomainEvents();
            Add(customFieldSetup);
        }
    }
}
