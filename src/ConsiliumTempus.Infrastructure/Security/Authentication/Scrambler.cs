using ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;
using BC = BCrypt.Net;

namespace ConsiliumTempus.Infrastructure.Security.Authentication;

public sealed class Scrambler : IScrambler
{
    public string HashPassword(string password)
    {
        return BC.BCrypt.EnhancedHashPassword(password, workFactor: 13);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BC.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}