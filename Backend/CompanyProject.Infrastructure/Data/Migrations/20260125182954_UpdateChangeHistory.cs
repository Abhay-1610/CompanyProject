using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChangeHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeHistories_Projects_ProjectId",
                table: "ChangeHistories");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "ChangeHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "ChangeHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeHistories_Projects_ProjectId",
                table: "ChangeHistories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeHistories_Projects_ProjectId",
                table: "ChangeHistories");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "ChangeHistories");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "ChangeHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeHistories_Projects_ProjectId",
                table: "ChangeHistories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
