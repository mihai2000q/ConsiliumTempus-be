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
        Guid token,
        Guid jwtId,
        DateTime expiryDate,
        bool invalidated,
        long usedTimes,
        UserAggregate user,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(token)
    {
        JwtId = jwtId;
        ExpiryDate = expiryDate;
        Invalidated = invalidated;
        UsedTimes = usedTimes;
        User = user;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public Guid JwtId { get; init; }
    public DateTime ExpiryDate { get; init; }
    public bool Invalidated { get; private set; }
    public long UsedTimes { get; private set; }
    public DateTime CreatedDateTime { get; init; }
    public DateTime UpdatedDateTime { get; private set; }
    public UserAggregate User { get; init; } = null!;

    public string Value => Id.ToString();

    public static RefreshToken Create(
        string token,
        string jwtId,
        UserAggregate user)
    {
        return new RefreshToken(
            new Guid(token),
            new Guid(jwtId),
            DateTime.UtcNow.AddMonths(1),
            false,
            0,
            user,
            DateTime.UtcNow, 
            DateTime.UtcNow);
    }

    public void UpdateUsage()
    {
        UsedTimes++;
        UpdatedDateTime = DateTime.UtcNow;
    }
}