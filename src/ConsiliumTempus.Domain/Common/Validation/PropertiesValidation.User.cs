namespace ConsiliumTempus.Domain.Common.Validation;

public static partial class PropertiesValidation
{
    public static class User
    {
        public const short FirstNameMaximumLength = 100;
        public const short LastNameMaximumLength = 100;
        public const short EmailMaximumLength = 100;
        public const short PlainPasswordMaximumLength = 100;
        public const short RoleMaximumLength = 50;
    }
}