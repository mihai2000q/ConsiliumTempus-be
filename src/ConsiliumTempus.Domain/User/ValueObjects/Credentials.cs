using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.User.ValueObjects;

public class Credentials : ValueObject
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")] // used by EF
    private Credentials()
    {
    }

    private Credentials(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; } = string.Empty;
    public string Password { get; } = string.Empty;

    public static Credentials Create(string email, string password)
    {
        return new Credentials(email, password);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
        yield return Password;
    }
}