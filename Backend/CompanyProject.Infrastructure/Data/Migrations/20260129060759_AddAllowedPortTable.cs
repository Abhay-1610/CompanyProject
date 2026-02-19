using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowedPortTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllowedPorts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Port = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedPorts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AllowedPorts",
                columns: new[] { "Id", "Port" },
                values: new object[] { 1, 3000 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowedPorts");
        }
    }
}
