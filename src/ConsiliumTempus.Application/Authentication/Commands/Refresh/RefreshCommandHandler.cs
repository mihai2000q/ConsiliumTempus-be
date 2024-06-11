using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using ConsiliumTempus.Domain.Authentication.ValueObjects;
using ConsiliumTempus.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Authentication.Commands.Refresh;

public sealed class RefreshCommandHandler(
    IJwtTokenValidator jwtTokenValidator,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<RefreshCommand, ErrorOr<RefreshResult>>
{
    public async Task<ErrorOr<RefreshResult>> Handle(RefreshCommand command, CancellationToken cancellationToken)
    {
        if (!await jwtTokenValidator.ValidateAccessToken(command.Token)) return Errors.Authentication.InvalidTokens;

        var refreshToken = await refreshTokenRepository.Get(
            RefreshTokenId.Create(command.RefreshToken),
            cancellationToken);

        if (refreshToken is null || !jwtTokenValidator.ValidateRefreshToken(refreshToken))
            return Errors.Authentication.InvalidTokens;

        var jwtId = JwtId.Create(jwtTokenGenerator.GetJwtIdFromToken(command.Token));

        string? token = null;
        if (refreshToken.JwtId == jwtId)
        {
            token = jwtTokenGenerator.GenerateToken(refreshToken.User);
            refreshToken.Refresh(JwtId.Create(jwtTokenGenerator.GetJwtIdFromToken(token)));
        }
        else if (refreshToken.HasRefreshed(jwtId))
        {
            token = jwtTokenGenerator.GenerateToken(refreshToken.User, refreshToken.JwtId.Value);
        }

        return token is not null ? new RefreshResult(token) : Errors.Authentication.InvalidTokens;
    }
}