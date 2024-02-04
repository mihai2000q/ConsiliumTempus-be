using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.UserAggregate.ValueObjects;

public class Credentials : ValueObject
{
    private Credentials()
    {
    }

    private Credentials(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;

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