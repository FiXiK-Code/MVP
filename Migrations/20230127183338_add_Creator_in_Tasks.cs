using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class add_Creator_in_Tasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "creator",
                table: "DBTask",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creator",
                table: "DBTask");
        }
    }
}
