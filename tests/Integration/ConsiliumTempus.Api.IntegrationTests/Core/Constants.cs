namespace ConsiliumTempus.Api.IntegrationTests.Core;

public static class Constants
{
    public const string MsSqlImage = "mcr.microsoft.com/mssql/server:2022-latest";
    public const string DatabasePassword = "StrongPassword123";
    
    public const string Environment = "Testing";
    
    public const string MockDirectoryPath = "../../../MockData/";
    public const string DefaultUsersFilePath = "../../../MockData/Users.sql";
}