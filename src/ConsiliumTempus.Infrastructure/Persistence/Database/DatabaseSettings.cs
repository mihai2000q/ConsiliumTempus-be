namespace ConsiliumTempus.Infrastructure.Persistence.Database;

public class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";
    public string Server { get; init; } = string.Empty;
    public string Port { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}