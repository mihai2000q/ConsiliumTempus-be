namespace ConsiliumTempus.Domain.Common.Validation;

public static partial class PropertiesValidation
{
    public static class ProjectTask
    {
        public const short NameMaximumLength = 100;
        public const short DescriptionMaximumLength = 1000;
    }

    public static class ProjectTaskComment
    {
        public const short MessageMaximumLength = 256;
    }
}