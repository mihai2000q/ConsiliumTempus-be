using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Authentication.ValueObjects;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Authentication;

public sealed class RefreshToken : Entity<RefreshTokenId>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private RefreshToken()
    {
    }

    private RefreshToken(
        RefreshTokenId id,
        DateTime expiryDateTime,
        IsInvalidated isInvalidated,
        UserAggregate user,
        DateTime createdDateTime,
        JwtId jwtId) : base(id)
    {
        ExpiryDateTime = expiryDateTime;
        IsInvalidated = isInvalidated;
        User = user;
        CreatedDateTime = createdDateTime;
        _history.Add(RefreshTokenHistory.Create(this, jwtId));
    }

    private readonly List<RefreshTokenHistory> _history = [];

    public DateTime ExpiryDateTime { get; init; }
    public IsInvalidated IsInvalidated { get; private set; } = default!;
    public DateTime CreatedDateTime { get; init; }
    public UserAggregate User { get; init; } = default!;
    public IReadOnlyList<RefreshTokenHistory> History => _history.AsReadOnly();
    public JwtId JwtId => _history[^1].JwtId;
    public DateTime UpdatedDateTime => _history[^1].CreatedDateTime;
    public int RefreshTimes => _history.Count - 1;

    public static RefreshToken Create(
        UserAggregate user,
        JwtId jwtId)
    {
        return new RefreshToken(
            RefreshTokenId.CreateUnique(), 
            DateTime.UtcNow.AddMonths(1),
            IsInvalidated.Create(false), 
            user,
            DateTime.UtcNow,
            jwtId);
    }

    public void Refresh(JwtId jwtId)
    {
        _history.Add(RefreshTokenHistory.Create(this, jwtId));
    }

    public bool HasRefreshed(JwtId jwtId)
    {
        return _history.SingleOrDefault(h => h.RefreshToken == this && h.JwtId == jwtId) is not null;
    }
}