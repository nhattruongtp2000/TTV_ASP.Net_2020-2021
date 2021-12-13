using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class idproductgiveto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdProductGiveTo",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdProductGiveTo",
                table: "Products");
        }
    }
}
