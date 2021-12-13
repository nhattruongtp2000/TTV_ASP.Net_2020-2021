using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class dsadsa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_IdProduct",
                table: "OrderDetails",
                column: "IdProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_IdProduct",
                table: "OrderDetails",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "IdProduct",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_IdProduct",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_IdProduct",
                table: "OrderDetails");
        }
    }
}
