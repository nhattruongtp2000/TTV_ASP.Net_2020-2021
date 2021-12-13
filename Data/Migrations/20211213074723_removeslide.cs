using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class removeslide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdBrand",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "IdCategory",
                table: "Slides");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdBrand",
                table: "Slides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdCategory",
                table: "Slides",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
