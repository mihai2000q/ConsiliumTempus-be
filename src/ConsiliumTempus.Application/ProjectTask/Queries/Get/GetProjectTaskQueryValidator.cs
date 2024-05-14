using FluentValidation;

namespace ConsiliumTempus.Application.ProjectTask.Queries.Get;

public sealed class GetProjectTaskQueryValidator : AbstractValidator<GetProjectTaskQuery>
{
    public GetProjectTaskQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}