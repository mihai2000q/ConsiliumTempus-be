using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class ProjectTask
    {
        public static Error NotFound => Error.NotFound(
            "ProjectTask.NotFound",
            "Project Task could not be found");
    }
}