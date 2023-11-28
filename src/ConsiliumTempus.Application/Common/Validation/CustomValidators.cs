using ConsiliumTempus.Application.Common.Extensions;
using FluentValidation;

namespace ConsiliumTempus.Application.Common.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> IsPassword<T>(this IRuleBuilder<T, string> ruleBuilder) 
    {
        return ruleBuilder.Must(p => p.Length > 8)
            .WithMessage("The {PropertyName} should be longer than 8 characters")
            .Must(p => p.ContainsUppercase())
            .WithMessage("The {PropertyName} should contain an uppercase letter")
            .Must(p => p.ContainsLowercase())
            .WithMessage("The {PropertyName} should contain a lowercase letter")
            .Must(p => p.ContainsNumber())
            .WithMessage("The {PropertyName} should contain at least a number");
    }
    
    public static IRuleBuilderOptions<T, string> IsEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(p => p.IsValidEmail())
            .WithMessage("The {PropertyName} is not valid");
    }
}