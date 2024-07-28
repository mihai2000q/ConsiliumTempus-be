using FluentValidation;

namespace ConsiliumTempus.Application.Project.Commands.UpdateFavorites;

public sealed class UpdateFavoritesProjectCommandValidator : AbstractValidator<UpdateFavoritesProjectCommand>
{
    public UpdateFavoritesProjectCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}