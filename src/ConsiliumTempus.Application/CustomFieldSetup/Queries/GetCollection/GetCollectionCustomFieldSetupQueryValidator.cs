using ConsiliumTempus.Application.Common.Extensions;
using FluentValidation;

namespace ConsiliumTempus.Application.CustomFieldSetup.Queries.GetCollection;

internal sealed class GetCollectionCustomFieldSetupQueryValidator : AbstractValidator<GetCollectionCustomFieldSetupQuery>
{
    public GetCollectionCustomFieldSetupQueryValidator()
    {
        RuleFor(c => c)
            .Must(c =>
                c.WorkspaceId.GetValueOrDefault() != Guid.Empty ||
                c.ProjectId.GetValueOrDefault() != Guid.Empty)
            .WithMessage("Either the 'WorkspaceId' or the 'ProjectId' must be set")
            .WithName(nameof(GetCollectionCustomFieldSetupQuery.WorkspaceId)
                .Dot(nameof(GetCollectionCustomFieldSetupQuery.ProjectId)));
    }
}