using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class ProjectSprint
    {
        public static Error NotFound => Error.NotFound(
            "ProjectSprint.NotFound",
            "Project Sprint could not be found");
    }

    public static class ProjectStage
    {
        public static Error NotFound => Error.NotFound(
            "ProjectStage.NotFound",
            "Project Stage could not be found");
    }
}