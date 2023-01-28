using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class add_timeWorck_in_Projects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "timeWork",
                table: "DBProject",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timeWork",
                table: "DBProject");
        }
    }
}
