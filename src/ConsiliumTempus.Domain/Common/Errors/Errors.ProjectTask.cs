using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class ProjectTask
    {
        public static Error NotFound => Error.NotFound(
            "ProjectTask.NotFound",
            "Project Task could not be found");

        public static Error OverNotFound => Error.NotFound(
            "ProjectTask.OverNotFound",
            "Project Task or Project Stage could not be found");
    }

    public static class CustomField
    {
        public static Error NotFound => Error.NotFound(
            "CustomField.NotFound",
            "Custom Field could not be found");

        public static Error TypeMismatch => Error.Conflict(
            "CustomField.TypeMismatch",
            "The type of the request does not match the type of the Custom Field");
    }
}