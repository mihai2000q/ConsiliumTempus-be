using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Common.UnitTests.Project;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Project;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.Create;

internal static class CreateProjectSprintCommandHandlerData
{
    internal class GetCommands : TheoryData<CreateProjectSprintCommand, ProjectAggregate>
    {
        public GetCommands()
        {
            var project = ProjectFactory.CreateWithSprints();
            
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand();
            Add(command, project);
            
            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                keepPreviousStages: true);
            Add(command, project);
            
            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                keepPreviousStages: true);
            project = ProjectFactory.Create();
            Add(command, project);
        }
    }
}