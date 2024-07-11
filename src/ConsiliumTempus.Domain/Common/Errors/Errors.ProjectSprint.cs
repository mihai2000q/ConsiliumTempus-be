using ConsiliumTempus.Domain.ProjectSprint.ValueObjects;
using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class ProjectSprint
    {
        public static Error NotFound => Error.NotFound(
            "ProjectSprint.NotFound",
            "Project Sprint could not be found");

        public static Error OnlyOneSprint => Error.NotFound(
            "ProjectSprint.OnlyOneSprint",
            "Each Project should have at least one project sprint");
    }

    public static class ProjectStage
    {
        public static Func<ProjectStageId, Error> NotFoundWithId => id => Error.NotFound(
            "ProjectStage.NotFound",
            $"Project Stage, {id}, could not be found");

        public static Error NotFound => Error.NotFound(
            "ProjectStage.NotFound",
            "Project Stage could not be found");

        public static Error OnlyOneStage => Error.NotFound(
            "ProjectStage.OnlyOneStage",
            "Each Project Sprint should have at least one project stage");
    }
}