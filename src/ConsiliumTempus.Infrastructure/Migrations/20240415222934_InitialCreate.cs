using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsiliumTempus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTaskComment_ProjectTask_TaskAggregateId",
                table: "ProjectTaskComment");

            migrationBuilder.RenameColumn(
                name: "TaskAggregateId",
                table: "ProjectTaskComment",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTaskComment_TaskAggregateId",
                table: "ProjectTaskComment",
                newName: "IX_ProjectTaskComment_TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTaskComment_ProjectTask_TaskId",
                table: "ProjectTaskComment",
                column: "TaskId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTaskComment_ProjectTask_TaskId",
                table: "ProjectTaskComment");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "ProjectTaskComment",
                newName: "TaskAggregateId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTaskComment_TaskId",
                table: "ProjectTaskComment",
                newName: "IX_ProjectTaskComment_TaskAggregateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTaskComment_ProjectTask_TaskAggregateId",
                table: "ProjectTaskComment",
                column: "TaskAggregateId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
