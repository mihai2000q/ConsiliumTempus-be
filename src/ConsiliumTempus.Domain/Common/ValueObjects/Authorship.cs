using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;
using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Domain.Common.ValueObjects;

/// <summary>
/// Who wrote something, and when it was written and last edited.
/// It is for what only its author can edit (i.e., a comment), so there is no one else to say who updated it.
/// It is a part of the <see cref="Interfaces.IAuthored"/> entity that holds it, and it is stamped
/// when the changes of that entity are saved, so the domain does not have to create or update it.
/// </summary>
public sealed class Authorship : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Authorship()
    {
    }

    private Authorship(
        UserAggregate? author,
        DateTime createdDateTime,
        DateTime updatedDateTime)
    {
        Author = author;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public UserAggregate? Author { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }

    public static Authorship Create(UserAggregate? author, DateTime createdDateTime)
    {
        return new Authorship(author, createdDateTime, createdDateTime);
    }

    public Authorship Update(DateTime updatedDateTime)
    {
        return new Authorship(Author, CreatedDateTime, updatedDateTime);
    }

    public Authorship ClearUser(UserAggregate user)
    {
        return new Authorship(
            Author == user ? null : Author,
            CreatedDateTime,
            UpdatedDateTime);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Author;
        yield return CreatedDateTime;
        yield return UpdatedDateTime;
    }
}
