using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChangedByEmailToChangeHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangedByEmail",
                table: "ChangeHistories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedByEmail",
                table: "ChangeHistories");
        }
    }
}
