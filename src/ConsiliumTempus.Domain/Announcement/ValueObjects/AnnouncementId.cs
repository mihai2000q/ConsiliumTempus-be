using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Announcement.ValueObjects;

public sealed class AnnouncementId : AggregateRootId<Guid>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private AnnouncementId()
    {
    }

    private AnnouncementId(Guid value)
    {
        Value = value;
    }

    public override Guid Value { get; protected set; }

    public static AnnouncementId CreateUnique()
    {
        return new AnnouncementId(Guid.NewGuid());
    }

    public static AnnouncementId Create(Guid value)
    {
        return new AnnouncementId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}