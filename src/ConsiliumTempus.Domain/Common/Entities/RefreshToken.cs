using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Interfaces;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Common.Entities;

public sealed class RefreshToken : Entity<Guid>, ITimestamps
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private RefreshToken()
    {
    }

    private RefreshToken(
        Guid id,
        Guid jwtId,
        DateTime expiryDateTime,
        bool isInvalidated,
        long refreshTimes,
        UserAggregate user,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(id)
    {
        JwtId = jwtId;
        ExpiryDateTime = expiryDateTime;
        IsInvalidated = isInvalidated;
        RefreshTimes = refreshTimes;
        User = user;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public Guid JwtId { get; private set; }
    public DateTime ExpiryDateTime { get; init; }
    public bool IsInvalidated { get; private set; }
    public long RefreshTimes { get; private set; }
    public UserAggregate User { get; init; } = null!;
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }

    public string Value => Id.ToString();

    public static RefreshToken Create(
        string jwtId,
        UserAggregate user)
    {
        return new RefreshToken(
            Guid.NewGuid(),
            new Guid(jwtId),
            DateTime.UtcNow.AddDays(7),
            false,
            0,
            user,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }

    public void Refresh(Guid jwtId)
    {
        JwtId = jwtId;
        RefreshTimes++;
        UpdatedDateTime = DateTime.UtcNow;
    }
}