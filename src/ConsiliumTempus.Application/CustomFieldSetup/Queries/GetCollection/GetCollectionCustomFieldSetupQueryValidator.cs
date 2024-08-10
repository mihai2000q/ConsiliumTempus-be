using ConsiliumTempus.Application.Common.Extensions;
using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;

public sealed class GetCollectionCustomFieldSetupQueryValidator : AbstractValidator<GetCollectionCustomFieldSetupQuery>
{
    public GetCollectionCustomFieldSetupQueryValidator()
    {
        RuleFor(c => c)
            .Must(c => c.WorkspaceId is not null || c.ProjectId is not null)
            .WithMessage("Either the 'WorkspaceId' or the 'ProjectId' must be set")
            .WithName(nameof(GetCollectionCustomFieldSetupQuery.WorkspaceId)
                .Dot(nameof(GetCollectionCustomFieldSetupQuery.ProjectId)));
    }
}