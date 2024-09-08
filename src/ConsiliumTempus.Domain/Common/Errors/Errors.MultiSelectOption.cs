using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class MultiSelectOption
    {
        public static Error NotFound => Error.NotFound(
            "MultiSelectOption.NotFound",
            "Multi Select Option could not be found");
    }
}