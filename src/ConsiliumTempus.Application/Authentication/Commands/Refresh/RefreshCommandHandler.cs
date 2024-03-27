using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
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
    public async Task<ErrorOr<RefreshResult>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        if (!await jwtTokenValidator.ValidateAccessToken(request.Token)) return Errors.Authentication.InvalidTokens;

        var refreshToken = await refreshTokenRepository.Get(request.RefreshToken, cancellationToken);

        if (!jwtTokenValidator.ValidateRefreshToken(refreshToken, jwtTokenGenerator.GetJwtIdFromToken(request.Token)))
            return Errors.Authentication.InvalidTokens;

        var token = jwtTokenGenerator.GenerateToken(refreshToken!.User);

        refreshToken.UpdateUsage();

        return new RefreshResult(token);
    }
}