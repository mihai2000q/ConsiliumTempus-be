using FluentValidation;

namespace ConsiliumTempus.Application.Project.Queries.GetAllowedMembers;

public sealed class GetAllowedMembersFromProjectQueryValidator : AbstractValidator<GetAllowedMembersFromProjectQuery>
{
    public GetAllowedMembersFromProjectQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}