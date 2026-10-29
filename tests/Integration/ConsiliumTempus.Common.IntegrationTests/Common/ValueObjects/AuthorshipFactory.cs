using ConsiliumTempus.Domain.Common.ValueObjects;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Common.IntegrationTests.Common.ValueObjects;

public static class AuthorshipFactory
{
    public static Authorship Create(
        UserAggregate author,
        DateTime? createdDateTime = null,
        DateTime? updatedDateTime = null)
    {
        return EntityBuilder<Authorship>.Empty()
            .WithProperty(nameof(Authorship.Author), author)
            .WithProperty(nameof(Authorship.CreatedDateTime), createdDateTime ?? DateTime.UtcNow)
            .WithProperty(nameof(Authorship.UpdatedDateTime), updatedDateTime ?? DateTime.UtcNow)
            .Build();
    }
}
