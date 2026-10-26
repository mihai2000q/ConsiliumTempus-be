using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.UpdateFavorites;

internal sealed class UpdateFavoritesProjectCommandValidator : AbstractValidator<UpdateFavoritesProjectCommand>
{
    public UpdateFavoritesProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}