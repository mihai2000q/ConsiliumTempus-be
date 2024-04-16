using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class Project
    {
        public static Error NotFound => Error.NotFound(
            "Project.NotFound",
            "Project could not be found");
    }

    public static class ProjectSprint
    {
        public static Error NotFound => Error.NotFound(
            "ProjectSprint.NotFound",
            "Project Sprint could not be found");
    }
}