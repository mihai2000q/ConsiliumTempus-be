using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class SingleSelectOption
    {
        public static Error NotFound => Error.NotFound(
            "SingleSelectOption.NotFound",
            "Single Select Option could not be found");
    }
}