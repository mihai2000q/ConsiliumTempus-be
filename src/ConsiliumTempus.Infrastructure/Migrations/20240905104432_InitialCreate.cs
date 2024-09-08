using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConsiliumTempus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Audit_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Audit_User_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsInvalidated = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPersonal = table.Column<bool>(type: "bit", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workspace_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceRoleHasPermission",
                columns: table => new
                {
                    WorkspaceRoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceRoleHasPermission", x => new { x.WorkspaceRoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_WorkspaceRoleHasPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceRoleHasPermission_WorkspaceRole_WorkspaceRoleId",
                        column: x => x.WorkspaceRoleId,
                        principalTable: "WorkspaceRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokenHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JwtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokenHistory_RefreshToken_RefreshTokenId",
                        column: x => x.RefreshTokenId,
                        principalTable: "RefreshToken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldSetup_Date",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldSetup_Date", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Date_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Date_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldSetup_DateTime",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldSetup_DateTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_DateTime_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_DateTime_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldSetup_Duration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultDuration = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldSetup_Duration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Duration_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Duration_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldSetup_MultiSelect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldSetup_MultiSelect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_MultiSelect_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_MultiSelect_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldSetup_Number",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Decimals = table.Column<short>(type: "smallint", maxLength: 9, nullable: false),
                    Rounding = table.Column<bool>(type: "bit", nullable: false),
                    DefaultNumber = table.Column<decimal>(type: "decimal(38,9)", precision: 38, scale: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldSetup_Number", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Number_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Number_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldSetup_People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldSetup_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_People_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_People_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldSetup_SingleSelect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldSetup_SingleSelect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_SingleSelect_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_SingleSelect_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldSetup_Text",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldSetup_Text", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Text_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Text_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomFieldSetup_Time",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultTime = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFieldSetup_Time", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Time_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomFieldSetup_Time_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Membership",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceRoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => new { x.UserId, x.WorkspaceId });
                    table.ForeignKey(
                        name: "FK_Membership_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Membership_WorkspaceRole_WorkspaceRoleId",
                        column: x => x.WorkspaceRoleId,
                        principalTable: "WorkspaceRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Membership_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lifecycle = table.Column<int>(type: "int", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Project_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHasFavoriteWorkspace",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHasFavoriteWorkspace", x => new { x.WorkspaceId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserHasFavoriteWorkspace_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHasFavoriteWorkspace_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceInvitation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollaboratorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceInvitation_User_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkspaceInvitation_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkspaceInvitation_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiSelectOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    CustomOrderPosition = table.Column<int>(type: "int", nullable: false),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiSelectOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultiSelectOption_CustomFieldSetup_MultiSelect_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_MultiSelect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SingleSelectOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    CustomOrderPosition = table.Column<int>(type: "int", nullable: false),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleSelectOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleSelectOption_CustomFieldSetup_SingleSelect_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_SingleSelect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectHasAllowedMember",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectHasAllowedMember", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectHasAllowedMember_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectHasAllowedMember_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectHasCustomFieldSetup",
                columns: table => new
                {
                    CustomFieldSetupsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectHasCustomFieldSetup", x => new { x.CustomFieldSetupsId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_ProjectHasCustomFieldSetup_Project_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSprint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSprint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSprint_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectSprint_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectStatus_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectStatus_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHasFavoriteProject",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHasFavoriteProject", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserHasFavoriteProject_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHasFavoriteProject_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomOrderPosition = table.Column<int>(type: "int", nullable: false),
                    SprintId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectStage_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectStage_ProjectSprint_SprintId",
                        column: x => x.SprintId,
                        principalTable: "ProjectSprint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomOrderPosition = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EstimatedDuration = table.Column<TimeSpan>(type: "time", nullable: true),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTask_ProjectStage_StageId",
                        column: x => x.StageId,
                        principalTable: "ProjectStage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTask_User_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectTask_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTask_User_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_Date",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_Date", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_Date_CustomFieldSetup_Date_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_Date",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_Date_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_DateTime",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_DateTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_DateTime_CustomFieldSetup_DateTime_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_DateTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_DateTime_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_Duration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: true),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_Duration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_Duration_CustomFieldSetup_Duration_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_Duration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_Duration_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_MultiSelect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_MultiSelect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_MultiSelect_CustomFieldSetup_MultiSelect_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_MultiSelect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_MultiSelect_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_Number",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<decimal>(type: "decimal(38,9)", precision: 38, scale: 9, nullable: true),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_Number", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_Number_CustomFieldSetup_Number_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_Number",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_Number_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_People_CustomFieldSetup_People_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_People_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomField_People_User_PersonId",
                        column: x => x.PersonId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_SingleSelect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_SingleSelect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_SingleSelect_CustomFieldSetup_SingleSelect_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_SingleSelect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_SingleSelect_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomField_SingleSelect_SingleSelectOption_OptionId",
                        column: x => x.OptionId,
                        principalTable: "SingleSelectOption",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_Text",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_Text", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_Text_CustomFieldSetup_Text_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_Text",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_Text_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_Time",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: true),
                    SetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_Time", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomField_Time_CustomFieldSetup_Time_SetupId",
                        column: x => x.SetupId,
                        principalTable: "CustomFieldSetup_Time",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_Time_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectTaskComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeSpent = table.Column<TimeSpan>(type: "time", nullable: true),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTaskComment_ProjectTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTaskComment_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomField_MultiSelectHasOption",
                columns: table => new
                {
                    OptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomField_MultiSelectHasOption", x => new { x.CustomFieldId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_CustomField_MultiSelectHasOption_CustomField_MultiSelect_CustomFieldId",
                        column: x => x.CustomFieldId,
                        principalTable: "CustomField_MultiSelect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomField_MultiSelectHasOption_MultiSelectOption_OptionId",
                        column: x => x.OptionId,
                        principalTable: "MultiSelectOption",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "CreateCustomFieldSetupOnProject" },
                    { 2, "CreateCustomFieldSetupOnWorkspace" },
                    { 3, "AddCustomFieldSetupToProject" },
                    { 4, "ReadCustomFieldSetup" },
                    { 5, "ReadCollectionCustomFieldSetupFromProject" },
                    { 6, "ReadCollectionCustomFieldSetupFromWorkspace" },
                    { 7, "UpdateCustomFieldSetup" },
                    { 8, "MakeCustomFieldSetupGlobal" },
                    { 9, "DeleteCustomFieldSetup" },
                    { 10, "RemoveCustomFieldSetupFromProject" },
                    { 11, "CreateProject" },
                    { 12, "ReadProject" },
                    { 13, "ReadOverviewProject" },
                    { 14, "ReadCollectionProject" },
                    { 15, "UpdateProject" },
                    { 16, "UpdateFavoritesProject" },
                    { 17, "UpdateOverviewProject" },
                    { 18, "DeleteProject" },
                    { 19, "ReadAllowedMembersFromProject" },
                    { 20, "AddStatusToProject" },
                    { 21, "ReadStatusesFromProject" },
                    { 22, "UpdateStatusFromProject" },
                    { 23, "RemoveStatusFromProject" },
                    { 24, "CreateProjectSprint" },
                    { 25, "ReadProjectSprint" },
                    { 26, "ReadCollectionProjectSprint" },
                    { 27, "UpdateProjectSprint" },
                    { 28, "DeleteProjectSprint" },
                    { 29, "ReadStagesFromProjectSprint" },
                    { 30, "AddStageToProjectSprint" },
                    { 31, "MoveStageFromProjectSprint" },
                    { 32, "UpdateStageFromProjectSprint" },
                    { 33, "RemoveStageFromProjectSprint" },
                    { 34, "CreateProjectTask" },
                    { 35, "ReadProjectTask" },
                    { 36, "ReadCollectionProjectTask" },
                    { 37, "MoveProjectTask" },
                    { 38, "UpdateProjectTask" },
                    { 39, "UpdateCustomFieldFromProjectTask" },
                    { 40, "UpdateIsCompletedProjectTask" },
                    { 41, "UpdateOverviewProjectTask" },
                    { 42, "DeleteProjectTask" },
                    { 43, "ReadWorkspace" },
                    { 44, "ReadOverviewWorkspace" },
                    { 45, "ReadInvitationsFromWorkspace" },
                    { 46, "UpdateWorkspace" },
                    { 47, "UpdateFavoritesWorkspace" },
                    { 48, "UpdateOverviewWorkspace" },
                    { 49, "DeleteWorkspace" },
                    { 50, "InviteCollaboratorToWorkspace" },
                    { 51, "ReadCollaboratorsFromWorkspace" },
                    { 52, "UpdateCollaboratorFromWorkspace" },
                    { 53, "KickCollaboratorFromWorkspace" }
                });

            migrationBuilder.InsertData(
                table: "WorkspaceRole",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "This role can only read data", "View" },
                    { 2, "This role can do most of the actions with some limitations", "Member" },
                    { 3, "This role can do everything", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "WorkspaceRoleHasPermission",
                columns: new[] { "PermissionId", "WorkspaceRoleId" },
                values: new object[,]
                {
                    { 12, 1 },
                    { 13, 1 },
                    { 14, 1 },
                    { 16, 1 },
                    { 21, 1 },
                    { 25, 1 },
                    { 26, 1 },
                    { 29, 1 },
                    { 35, 1 },
                    { 36, 1 },
                    { 43, 1 },
                    { 44, 1 },
                    { 47, 1 },
                    { 51, 1 },
                    { 4, 2 },
                    { 5, 2 },
                    { 6, 2 },
                    { 12, 2 },
                    { 13, 2 },
                    { 14, 2 },
                    { 15, 2 },
                    { 16, 2 },
                    { 17, 2 },
                    { 19, 2 },
                    { 21, 2 },
                    { 22, 2 },
                    { 25, 2 },
                    { 26, 2 },
                    { 27, 2 },
                    { 29, 2 },
                    { 32, 2 },
                    { 34, 2 },
                    { 35, 2 },
                    { 36, 2 },
                    { 37, 2 },
                    { 38, 2 },
                    { 39, 2 },
                    { 40, 2 },
                    { 41, 2 },
                    { 42, 2 },
                    { 43, 2 },
                    { 44, 2 },
                    { 46, 2 },
                    { 47, 2 },
                    { 48, 2 },
                    { 51, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 3 },
                    { 5, 3 },
                    { 6, 3 },
                    { 7, 3 },
                    { 8, 3 },
                    { 9, 3 },
                    { 10, 3 },
                    { 11, 3 },
                    { 12, 3 },
                    { 13, 3 },
                    { 14, 3 },
                    { 15, 3 },
                    { 16, 3 },
                    { 17, 3 },
                    { 18, 3 },
                    { 19, 3 },
                    { 20, 3 },
                    { 21, 3 },
                    { 22, 3 },
                    { 23, 3 },
                    { 24, 3 },
                    { 25, 3 },
                    { 26, 3 },
                    { 27, 3 },
                    { 28, 3 },
                    { 29, 3 },
                    { 30, 3 },
                    { 31, 3 },
                    { 32, 3 },
                    { 33, 3 },
                    { 34, 3 },
                    { 35, 3 },
                    { 36, 3 },
                    { 37, 3 },
                    { 38, 3 },
                    { 39, 3 },
                    { 40, 3 },
                    { 41, 3 },
                    { 42, 3 },
                    { 43, 3 },
                    { 44, 3 },
                    { 45, 3 },
                    { 46, 3 },
                    { 47, 3 },
                    { 48, 3 },
                    { 49, 3 },
                    { 50, 3 },
                    { 51, 3 },
                    { 52, 3 },
                    { 53, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audit_CreatedById",
                table: "Audit",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_UpdatedById",
                table: "Audit",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Date_ProjectTaskId",
                table: "CustomField_Date",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Date_SetupId",
                table: "CustomField_Date",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_DateTime_ProjectTaskId",
                table: "CustomField_DateTime",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_DateTime_SetupId",
                table: "CustomField_DateTime",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Duration_ProjectTaskId",
                table: "CustomField_Duration",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Duration_SetupId",
                table: "CustomField_Duration",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_MultiSelect_ProjectTaskId",
                table: "CustomField_MultiSelect",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_MultiSelect_SetupId",
                table: "CustomField_MultiSelect",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_MultiSelectHasOption_OptionId",
                table: "CustomField_MultiSelectHasOption",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Number_ProjectTaskId",
                table: "CustomField_Number",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Number_SetupId",
                table: "CustomField_Number",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_People_PersonId",
                table: "CustomField_People",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_People_ProjectTaskId",
                table: "CustomField_People",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_People_SetupId",
                table: "CustomField_People",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_SingleSelect_OptionId",
                table: "CustomField_SingleSelect",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_SingleSelect_ProjectTaskId",
                table: "CustomField_SingleSelect",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_SingleSelect_SetupId",
                table: "CustomField_SingleSelect",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Text_ProjectTaskId",
                table: "CustomField_Text",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Text_SetupId",
                table: "CustomField_Text",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Time_ProjectTaskId",
                table: "CustomField_Time",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_Time_SetupId",
                table: "CustomField_Time",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Date_AuditId",
                table: "CustomFieldSetup_Date",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Date_WorkspaceId",
                table: "CustomFieldSetup_Date",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_DateTime_AuditId",
                table: "CustomFieldSetup_DateTime",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_DateTime_WorkspaceId",
                table: "CustomFieldSetup_DateTime",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Duration_AuditId",
                table: "CustomFieldSetup_Duration",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Duration_WorkspaceId",
                table: "CustomFieldSetup_Duration",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_MultiSelect_AuditId",
                table: "CustomFieldSetup_MultiSelect",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_MultiSelect_WorkspaceId",
                table: "CustomFieldSetup_MultiSelect",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Number_AuditId",
                table: "CustomFieldSetup_Number",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Number_WorkspaceId",
                table: "CustomFieldSetup_Number",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_People_AuditId",
                table: "CustomFieldSetup_People",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_People_WorkspaceId",
                table: "CustomFieldSetup_People",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_SingleSelect_AuditId",
                table: "CustomFieldSetup_SingleSelect",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_SingleSelect_WorkspaceId",
                table: "CustomFieldSetup_SingleSelect",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Text_AuditId",
                table: "CustomFieldSetup_Text",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Text_WorkspaceId",
                table: "CustomFieldSetup_Text",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Time_AuditId",
                table: "CustomFieldSetup_Time",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldSetup_Time_WorkspaceId",
                table: "CustomFieldSetup_Time",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_WorkspaceId",
                table: "Membership",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_WorkspaceRoleId",
                table: "Membership",
                column: "WorkspaceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MultiSelectOption_SetupId",
                table: "MultiSelectOption",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_OwnerId",
                table: "Project",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_WorkspaceId",
                table: "Project",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectHasAllowedMember_UserId",
                table: "ProjectHasAllowedMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectHasCustomFieldSetup_ProjectsId",
                table: "ProjectHasCustomFieldSetup",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSprint_AuditId",
                table: "ProjectSprint",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSprint_ProjectId",
                table: "ProjectSprint",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStage_AuditId",
                table: "ProjectStage",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStage_SprintId",
                table: "ProjectStage",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatus_AuditId",
                table: "ProjectStatus",
                column: "AuditId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatus_ProjectId",
                table: "ProjectStatus",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_AssigneeId",
                table: "ProjectTask",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_CreatedById",
                table: "ProjectTask",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_ReviewerId",
                table: "ProjectTask",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_StageId",
                table: "ProjectTask",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskComment_CreatedById",
                table: "ProjectTaskComment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskComment_TaskId",
                table: "ProjectTaskComment",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenHistory_RefreshTokenId",
                table: "RefreshTokenHistory",
                column: "RefreshTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleSelectOption_SetupId",
                table: "SingleSelectOption",
                column: "SetupId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserHasFavoriteProject_UserId",
                table: "UserHasFavoriteProject",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHasFavoriteWorkspace_UserId",
                table: "UserHasFavoriteWorkspace",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Workspace_OwnerId",
                table: "Workspace",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceInvitation_CollaboratorId",
                table: "WorkspaceInvitation",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceInvitation_SenderId",
                table: "WorkspaceInvitation",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceInvitation_WorkspaceId",
                table: "WorkspaceInvitation",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceRoleHasPermission_PermissionId",
                table: "WorkspaceRoleHasPermission",
                column: "PermissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomField_Date");

            migrationBuilder.DropTable(
                name: "CustomField_DateTime");

            migrationBuilder.DropTable(
                name: "CustomField_Duration");

            migrationBuilder.DropTable(
                name: "CustomField_MultiSelectHasOption");

            migrationBuilder.DropTable(
                name: "CustomField_Number");

            migrationBuilder.DropTable(
                name: "CustomField_People");

            migrationBuilder.DropTable(
                name: "CustomField_SingleSelect");

            migrationBuilder.DropTable(
                name: "CustomField_Text");

            migrationBuilder.DropTable(
                name: "CustomField_Time");

            migrationBuilder.DropTable(
                name: "Membership");

            migrationBuilder.DropTable(
                name: "ProjectHasAllowedMember");

            migrationBuilder.DropTable(
                name: "ProjectHasCustomFieldSetup");

            migrationBuilder.DropTable(
                name: "ProjectStatus");

            migrationBuilder.DropTable(
                name: "ProjectTaskComment");

            migrationBuilder.DropTable(
                name: "RefreshTokenHistory");

            migrationBuilder.DropTable(
                name: "UserHasFavoriteProject");

            migrationBuilder.DropTable(
                name: "UserHasFavoriteWorkspace");

            migrationBuilder.DropTable(
                name: "WorkspaceInvitation");

            migrationBuilder.DropTable(
                name: "WorkspaceRoleHasPermission");

            migrationBuilder.DropTable(
                name: "CustomFieldSetup_Date");

            migrationBuilder.DropTable(
                name: "CustomFieldSetup_DateTime");

            migrationBuilder.DropTable(
                name: "CustomFieldSetup_Duration");

            migrationBuilder.DropTable(
                name: "CustomField_MultiSelect");

            migrationBuilder.DropTable(
                name: "MultiSelectOption");

            migrationBuilder.DropTable(
                name: "CustomFieldSetup_Number");

            migrationBuilder.DropTable(
                name: "CustomFieldSetup_People");

            migrationBuilder.DropTable(
                name: "SingleSelectOption");

            migrationBuilder.DropTable(
                name: "CustomFieldSetup_Text");

            migrationBuilder.DropTable(
                name: "CustomFieldSetup_Time");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "WorkspaceRole");

            migrationBuilder.DropTable(
                name: "ProjectTask");

            migrationBuilder.DropTable(
                name: "CustomFieldSetup_MultiSelect");

            migrationBuilder.DropTable(
                name: "CustomFieldSetup_SingleSelect");

            migrationBuilder.DropTable(
                name: "ProjectStage");

            migrationBuilder.DropTable(
                name: "ProjectSprint");

            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Workspace");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
