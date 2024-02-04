using ConsiliumTempus.Application.Common.Extensions;
using FluentValidation;

namespace ConsiliumTempus.Application.Common.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> IsPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(p => p.Length > 8)
            .WithMessage("'{PropertyName}' must be longer than 8 characters")
            .Must(p => p.ContainsUppercase())
            .WithMessage("'{PropertyName}' must contain an uppercase letter")
            .Must(p => p.ContainsLowercase())
            .WithMessage("'{PropertyName}' must contain a lowercase letter")
            .Must(p => p.ContainsNumber())
            .WithMessage("'{PropertyName}' must contain at least a number");
    }

    public static IRuleBuilderOptions<T, string> IsEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(p => p.IsValidEmail())
            .WithMessage("'{PropertyName}' must be valid");
    }
}