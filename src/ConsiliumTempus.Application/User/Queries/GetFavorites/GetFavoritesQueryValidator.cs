using FluentValidation;

namespace ConsiliumTempus.Application.User.Queries.GetFavorites;

public sealed class GetFavoritesQueryValidator : AbstractValidator<GetFavoritesQuery>
{
    public GetFavoritesQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .GreaterThan(0);

        RuleFor(q => q.CurrentPage)
            .GreaterThan(0);
    }
}