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
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand();
            var project = ProjectFactory.CreateWithSprints();
            Add(command, project);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                keepPreviousStages: true);
            project = ProjectFactory.CreateWithSprints();
            Add(command, project);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                keepPreviousStages: true);
            project = ProjectFactory.CreateWithSprints(sprintEndDate: new DateOnly(2024, 10, 13));
            Add(command, project);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                keepPreviousStages: true);
            project = ProjectFactory.Create();
            Add(command, project);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                startDate: new DateOnly(2024, 10, 12));
            project = ProjectFactory.Create();
            Add(command, project);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                startDate: new DateOnly(2024, 10, 12));
            project = ProjectFactory.CreateWithSprints(sprintEndDate: new DateOnly(2024, 10, 13));
            Add(command, project);
            
            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                projectStatus: ProjectSprintCommandFactory.CreateCreateProjectStatus());
            project = ProjectFactory.CreateWithSprints(sprintEndDate: new DateOnly(2024, 10, 13));
            Add(command, project);
        }
    }
}