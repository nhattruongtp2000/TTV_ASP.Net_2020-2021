using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ssss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_IpAccesses",
                table: "IpAccesses",
                column: "DateAccess");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IpAccesses",
                table: "IpAccesses");
        }
    }
}
