using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeHistories_Companies_CompanyId",
                table: "ChangeHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeHistories_Projects_ProjectId",
                table: "ChangeHistories");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "ChangeHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeHistories_Companies_CompanyId",
                table: "ChangeHistories",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeHistories_Projects_ProjectId",
                table: "ChangeHistories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeHistories_Companies_CompanyId",
                table: "ChangeHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeHistories_Projects_ProjectId",
                table: "ChangeHistories");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "ChangeHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeHistories_Companies_CompanyId",
                table: "ChangeHistories",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeHistories_Projects_ProjectId",
                table: "ChangeHistories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
