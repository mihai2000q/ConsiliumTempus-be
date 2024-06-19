using ConsiliumTempus.Application.Common.Extensions;
using ConsiliumTempus.Domain.Common.Filters;
using FluentValidation;

namespace ConsiliumTempus.Application.ProjectSprint.Queries.GetCollection;

public sealed class GetCollectionProjectSprintQueryValidator : AbstractValidator<GetCollectionProjectSprintQuery>
{
    public GetCollectionProjectSprintQueryValidator()
    {
        RuleFor(q => q.ProjectId)
            .NotEmpty();

        RuleFor(q => q.Search)
            .HasSearchFormat(ProjectSprintFilter.FilterProperties);
    }
}