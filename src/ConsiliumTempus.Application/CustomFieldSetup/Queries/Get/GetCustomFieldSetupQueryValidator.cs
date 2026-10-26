using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;

internal sealed class GetCustomFieldSetupQueryValidator : AbstractValidator<GetCustomFieldSetupQuery>
{
    public GetCustomFieldSetupQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}