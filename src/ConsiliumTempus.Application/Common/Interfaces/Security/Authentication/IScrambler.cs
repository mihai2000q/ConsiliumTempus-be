namespace ConsiliumTempus.Application.Common.Interfaces.Security.Authentication;

public interface IScrambler
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string hashedPassword);
}