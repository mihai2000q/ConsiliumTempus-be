namespace ConsiliumTempus.Domain.Common.Validation;

public static partial class PropertiesValidation
{
    public static class Project
    {
        public const short NameMaximumLength = 100;
        public const short DescriptionMaximumLength = 1000;
    }

    public static class ProjectSprint
    {
        public const short NameMaximumLength = 100;
    }

    public static class ProjectSection
    {
        public const short NameMaximumLength = 100;
    }
}