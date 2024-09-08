using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class CustomFieldSetup
    {
        public static Error NotFound => Error.NotFound(
            "CustomFieldSetup.NotFound",
            "Custom Field Setup could not be found");

        public static Error NotGlobal => Error.Conflict(
            "CustomFieldSetup.NotGlobal",
            "Custom Field Setup is not globally available in the workspace");

        public static Error AlreadyGlobal => Error.Conflict(
            "CustomFieldSetup.AlreadyGlobal",
            "This Custom Field Setup is already globally available in the workspace");

        public static Error ProjectAlreadyPresent => Error.Conflict(
            "CustomFieldSetup.ProjectAlreadyPresent",
            "Custom Field Setup already has the project");
    }
}