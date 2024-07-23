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
                name: "Membership",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkspaceRoleId = table.Column<int>(type: "int", nullable: false)
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
                    FavoritesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHasFavoriteWorkspace", x => new { x.FavoritesId, x.WorkspaceId });
                    table.ForeignKey(
                        name: "FK_UserHasFavoriteWorkspace_User_FavoritesId",
                        column: x => x.FavoritesId,
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
                    FavoritesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHasFavoriteProject", x => new { x.FavoritesId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_UserHasFavoriteProject_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHasFavoriteProject_User_FavoritesId",
                        column: x => x.FavoritesId,
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ReadWorkspace" },
                    { 2, "ReadOverviewWorkspace" },
                    { 3, "ReadCollaboratorsFromWorkspace" },
                    { 4, "ReadInvitationsWorkspace" },
                    { 5, "InviteCollaboratorToWorkspace" },
                    { 6, "UpdateWorkspace" },
                    { 7, "UpdateFavoritesWorkspace" },
                    { 8, "UpdateOverviewWorkspace" },
                    { 9, "DeleteWorkspace" },
                    { 10, "CreateProject" },
                    { 11, "ReadProject" },
                    { 12, "ReadOverviewProject" },
                    { 13, "ReadCollectionProject" },
                    { 14, "UpdateProject" },
                    { 15, "UpdateFavoritesProject" },
                    { 16, "UpdateOverviewProject" },
                    { 17, "DeleteProject" },
                    { 18, "AddStatusToProject" },
                    { 19, "ReadStatusesFromProject" },
                    { 20, "UpdateStatusFromProject" },
                    { 21, "RemoveStatusFromProject" },
                    { 22, "CreateProjectSprint" },
                    { 23, "ReadProjectSprint" },
                    { 24, "ReadCollectionProjectSprint" },
                    { 25, "UpdateProjectSprint" },
                    { 26, "DeleteProjectSprint" },
                    { 27, "ReadStagesFromProjectSprint" },
                    { 28, "AddStageToProjectSprint" },
                    { 29, "MoveStageFromProjectSprint" },
                    { 30, "UpdateStageFromProjectSprint" },
                    { 31, "RemoveStageFromProjectSprint" },
                    { 32, "CreateProjectTask" },
                    { 33, "ReadProjectTask" },
                    { 34, "ReadCollectionProjectTask" },
                    { 35, "MoveProjectTask" },
                    { 36, "UpdateProjectTask" },
                    { 37, "UpdateIsCompletedProjectTask" },
                    { 38, "UpdateOverviewProjectTask" },
                    { 39, "DeleteProjectTask" }
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
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 7, 1 },
                    { 11, 1 },
                    { 12, 1 },
                    { 13, 1 },
                    { 15, 1 },
                    { 19, 1 },
                    { 23, 1 },
                    { 24, 1 },
                    { 27, 1 },
                    { 33, 1 },
                    { 34, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 6, 2 },
                    { 7, 2 },
                    { 8, 2 },
                    { 11, 2 },
                    { 12, 2 },
                    { 13, 2 },
                    { 14, 2 },
                    { 15, 2 },
                    { 16, 2 },
                    { 19, 2 },
                    { 20, 2 },
                    { 23, 2 },
                    { 24, 2 },
                    { 25, 2 },
                    { 27, 2 },
                    { 30, 2 },
                    { 32, 2 },
                    { 33, 2 },
                    { 34, 2 },
                    { 36, 2 },
                    { 37, 2 },
                    { 38, 2 },
                    { 39, 2 },
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
                    { 13, 3 },
                    { 14, 3 },
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
                    { 36, 3 },
                    { 37, 3 },
                    { 38, 3 },
                    { 39, 3 }
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
                name: "IX_Membership_WorkspaceId",
                table: "Membership",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_WorkspaceRoleId",
                table: "Membership",
                column: "WorkspaceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_OwnerId",
                table: "Project",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_WorkspaceId",
                table: "Project",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSprint_AuditId",
                table: "ProjectSprint",
                column: "AuditId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSprint_ProjectId",
                table: "ProjectSprint",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStage_AuditId",
                table: "ProjectStage",
                column: "AuditId");

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
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserHasFavoriteProject_ProjectId",
                table: "UserHasFavoriteProject",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHasFavoriteWorkspace_WorkspaceId",
                table: "UserHasFavoriteWorkspace",
                column: "WorkspaceId");

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
                name: "Membership");

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
                name: "ProjectTask");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "WorkspaceRole");

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
