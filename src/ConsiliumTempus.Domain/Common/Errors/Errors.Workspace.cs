using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class Workspace
    {
        public static Error NotFound => Error.NotFound(
            "Workspace.NotFound",
            "Workspace could not be found");

        public static Error PersonalWorkspace => Error.Conflict(
            "Workspace.PersonalWorkspace",
            "Personal Workspaces cannot be deleted, not even by their owners");

        public static Error CollaboratorNotFound => Error.NotFound(
            "Workspace.CollaboratorNotFound",
            "Collaborator could not be found inside workspace");
    }

    public static class WorkspaceInvitation
    {
        public static Error NotFound => Error.NotFound(
            "WorkspaceInvitation.NotFound",
            "Workspace Invitation could not be found");

        public static Error AlreadyInvited => Error.NotFound(
            "WorkspaceInvitation.AlreadyInvited",
            "This user has already been invited");

        public static Error AlreadyCollaborator => Error.NotFound(
            "WorkspaceInvitation.AlreadyCollaborator",
            "This user is already a collaborator");
    }
}