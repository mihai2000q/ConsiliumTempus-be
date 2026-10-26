using FluentValidation;

namespace ConsiliumTempus.Application.Workspace.Commands.UpdateFavorites;

internal sealed class UpdateFavoriteWorkspaceCommandValidator : AbstractValidator<UpdateFavoritesWorkspaceCommand>
{
    public UpdateFavoriteWorkspaceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}