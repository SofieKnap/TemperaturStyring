using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemperaturStyring.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Thermometers",
                columns: table => new
                {
                    ThermometerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentTemp = table.Column<double>(type: "float", nullable: false),
                    MinTemp = table.Column<double>(type: "float", nullable: false),
                    MaxTemp = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thermometers", x => x.ThermometerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Thermometers");
        }
    }
}
