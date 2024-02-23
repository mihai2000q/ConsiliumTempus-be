using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Domain.Common.Relations;
using Respawn.Graph;

namespace ConsiliumTempus.Api.IntegrationTests.Core;

public static class SetupConstants
{
    public const string MsSqlImage = "mcr.microsoft.com/mssql/server:2022-latest";
    public const string DatabasePassword = "StrongPassword123";
    
    public const string Environment = "Testing";
    
    public const string MockDirectoryPath = "../../../TestData/";
    public const string DefaultUsersFilePath = "../../../TestData/User.sql";

    public static readonly Table[] TablesToIgnore = [
        nameof(Permission), nameof(WorkspaceRole), nameof(WorkspaceRoleHasPermission)
    ];
}