using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class add_HistoryWork_to_Tasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "historyWorc",
                table: "DBTask",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "historyWorc",
                table: "DBTask");
        }
    }
}
