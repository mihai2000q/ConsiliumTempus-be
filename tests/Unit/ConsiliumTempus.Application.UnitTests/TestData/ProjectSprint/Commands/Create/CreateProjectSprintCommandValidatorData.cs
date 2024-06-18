using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Application.ProjectSprint.Commands.Create;
using ConsiliumTempus.Common.UnitTests.ProjectSprint;
using ConsiliumTempus.Domain.Common.Validation;
using ConsiliumTempus.Domain.Project.Enums;

namespace ConsiliumTempus.Application.UnitTests.TestData.ProjectSprint.Commands.Create;

internal static class CreateProjectSprintCommandValidatorData
{
    internal class GetValidCommands : TheoryData<CreateProjectSprintCommand>
    {
        public GetValidCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand();
            Add(command);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                startDate: new DateOnly(2022, 10, 10));
            Add(command);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                endDate: new DateOnly(2022, 10, 10));
            Add(command);
            
            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                projectStatus: ProjectSprintCommandFactory.CreateCreateProjectStatus());
            Add(command);

            command = new CreateProjectSprintCommand(
                Guid.NewGuid(),
                "New Project Sprint",
                new DateOnly(2022, 11, 12),
                new DateOnly(2022, 11, 26),
                true,
                new CreateProjectSprintCommand.CreateProjectStatus(
                    "Status Update",
                    ProjectStatusType.Completed.ToString(),
                    "This is the new description of the new sprint that has just ended"));
            Add(command);
        }
    }

    internal class GetInvalidProjectIdCommands : TheoryData<CreateProjectSprintCommand, string, short>
    {
        public GetInvalidProjectIdCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(projectId: Guid.Empty);
            Add(command, nameof(command.ProjectId), 1);
        }
    }

    internal class GetInvalidNameCommands : TheoryData<CreateProjectSprintCommand, string, short>
    {
        public GetInvalidNameCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(name: "");
            Add(command, nameof(command.Name), 1);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                name: new string('a', PropertiesValidation.ProjectSprint.NameMaximumLength + 1));
            Add(command, nameof(command.Name), 1);
        }
    }

    internal class GetInvalidStartEndDateCommands : TheoryData<CreateProjectSprintCommand, string, short>
    {
        public GetInvalidStartEndDateCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                startDate: new DateOnly(2022, 10, 10),
                endDate: new DateOnly(2022, 10, 10));
            Add(command, nameof(command.StartDate).And(nameof(command.EndDate)), 1);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                startDate: new DateOnly(2022, 10, 10),
                endDate: new DateOnly(2022, 10, 9));
            Add(command, nameof(command.StartDate).And(nameof(command.EndDate)), 1);
        }
    }
    
    internal class GetInvalidProjectStatusTitleCommands : TheoryData<CreateProjectSprintCommand, string, short>
    {
        public GetInvalidProjectStatusTitleCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                projectStatus: ProjectSprintCommandFactory.CreateCreateProjectStatus(
                    title: ""));
            Add(command, nameof(command.ProjectStatus).Dot(nameof(command.ProjectStatus.Title)), 1);

            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                projectStatus: ProjectSprintCommandFactory.CreateCreateProjectStatus(
                    title: new string('a', PropertiesValidation.ProjectStatus.TitleMaximumLength + 1)));
            Add(command, nameof(command.ProjectStatus).Dot(nameof(command.ProjectStatus.Title)), 1);
        }
    }
    
    internal class GetInvalidProjectStatusStatusCommands : TheoryData<CreateProjectSprintCommand, string, short>
    {
        public GetInvalidProjectStatusStatusCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                projectStatus: ProjectSprintCommandFactory.CreateCreateProjectStatus(
                    status: ""));
            Add(command, nameof(command.ProjectStatus).Dot(nameof(command.ProjectStatus.Status)), 2);
            
            command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                projectStatus: ProjectSprintCommandFactory.CreateCreateProjectStatus(
                    status: "something"));
            Add(command, nameof(command.ProjectStatus).Dot(nameof(command.ProjectStatus.Status)), 1);
        }
    }
    
    internal class GetInvalidProjectStatusDescriptionCommands : TheoryData<CreateProjectSprintCommand, string, short>
    {
        public GetInvalidProjectStatusDescriptionCommands()
        {
            var command = ProjectSprintCommandFactory.CreateCreateProjectSprintCommand(
                projectStatus: ProjectSprintCommandFactory.CreateCreateProjectStatus(
                    description: ""));
            Add(command, nameof(command.ProjectStatus).Dot(nameof(command.ProjectStatus.Description)), 1);
        }
    }
}