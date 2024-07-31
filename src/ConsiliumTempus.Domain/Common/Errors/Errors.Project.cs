using ErrorOr;

namespace ConsiliumTempus.Domain.Common.Errors;

public static partial class Errors
{
    public static class Project
    {
        public static Error NotFound => Error.NotFound(
            "Project.NotFound",
            "Project could not be found");

        public static Error AllowedMemberNotFound => Error.NotFound(
            "Project.AllowedMemberNotFound",
            "Allowed Member could not be found");

        public static Error NotPrivate => Error.Conflict(
            "Project.NotPrivate",
            "Project is not private");

        public static Error AlreadyAllowedMember => Error.Conflict(
            "Project.AlreadyAllowedMember",
            "This user is already an allowed member");

        public static Error RemoveYourself => Error.Conflict(
            "Project.RemoveYourself",
            "You cannot remove yourself from allowed members");
    }
    
    public static class ProjectStatus
    {
        public static Error NotFound => Error.NotFound(
            "ProjectStatus.NotFound",
            "Project status could not be found");
    }
}