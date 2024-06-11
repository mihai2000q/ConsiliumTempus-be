using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Authentication.ValueObjects;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Authentication;

public sealed class RefreshTokenHistory : Entity<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private RefreshTokenHistory()
    {
    }

    private RefreshTokenHistory(
        Guid id,
        RefreshToken refreshToken,
        JwtId jwtId,
        DateTime createdDateTime) : base(id)
    {
        RefreshToken = refreshToken;
        JwtId = jwtId;
        CreatedDateTime = createdDateTime;
    }

    public JwtId JwtId { get; init; } = default!;
    public DateTime CreatedDateTime { get; init; }
    public RefreshToken RefreshToken { get; init; } = default!;
    
    public static RefreshTokenHistory Create(
        RefreshToken refreshToken,
        JwtId jwtId)
    {
        return new RefreshTokenHistory(
            Guid.NewGuid(),
            refreshToken,
            jwtId,
            DateTime.UtcNow);
    }
}