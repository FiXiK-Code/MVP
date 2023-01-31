using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class add_mail_to_Staff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mail",
                table: "DBStaff",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mail",
                table: "DBStaff");
        }
    }
}
