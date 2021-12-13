using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class changealias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdProduct",
                table: "Slides");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Slides",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Slides");

            migrationBuilder.AddColumn<int>(
                name: "IdProduct",
                table: "Slides",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
