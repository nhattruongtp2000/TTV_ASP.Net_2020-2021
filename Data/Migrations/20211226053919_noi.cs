using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class noi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhotos_IdProduct",
                table: "ProductPhotos",
                column: "IdProduct");


            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhotos_Products_IdProduct",
                table: "ProductPhotos",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "IdProduct",
                onDelete: ReferentialAction.Cascade);

  
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Products_IdCategory",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhotos_Products_IdProduct",
                table: "ProductPhotos");

            migrationBuilder.DropIndex(
                name: "IX_ProductPhotos_IdProduct",
                table: "ProductPhotos");

            migrationBuilder.AlterColumn<int>(
                name: "IdCategory",
                table: "Categories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
