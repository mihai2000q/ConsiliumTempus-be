using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Queries.Get;

public sealed class GetCustomFieldSetupQueryValidator : AbstractValidator<GetCustomFieldSetupQuery>
{
    public GetCustomFieldSetupQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}