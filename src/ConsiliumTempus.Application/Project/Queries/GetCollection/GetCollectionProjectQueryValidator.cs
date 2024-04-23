using ConsiliumTempus.Domain.Common.Validation;
using FluentValidation;

namespace ConsiliumTempus.Application.Project.Queries.GetCollection;

public sealed class GetCollectionProjectQueryValidator : AbstractValidator<GetCollectionProjectQuery>
{
    public GetCollectionProjectQueryValidator()
    {
        RuleFor(q => q.Name)
            .MaximumLength(PropertiesValidation.Project.NameMaximumLength);
    }
}