using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class CustomFieldSetup
    {
        public static Error NotFound => Error.NotFound(
            "CustomFieldSetup.NotFound",
            "Custom Field Setup could not be found");
    }
}