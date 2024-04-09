using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class Workspace
    {
        public static Error NotFound => Error.NotFound(
            "Workspace.NotFound",
            "Workspace could not be found");
        
        public static Error UserWorkspace => Error.Conflict(
            "Workspace.UserWorkspace",
            "User Workspaces cannot be deleted, not even by their owners");
    }
}