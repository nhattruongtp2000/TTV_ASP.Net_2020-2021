using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accesses");

            migrationBuilder.CreateTable(
                name: "IpAccesses",
                columns: table => new
                {
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAccess = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CountAcess = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IpAccesses");

            migrationBuilder.CreateTable(
                name: "Accesses",
                columns: table => new
                {
                    DateAcess = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfAccess = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesses", x => x.DateAcess);
                });
        }
    }
}
