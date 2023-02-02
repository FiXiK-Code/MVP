using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class redact_priority_0000ТСП : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DBProject",
                keyColumn: "code",
                keyValue: "00/00 - ТСП",
                column: "priority",
                value: "2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
