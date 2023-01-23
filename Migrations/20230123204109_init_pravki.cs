using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class init_pravki : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "supervisorId",
                table: "DBStaff");

            migrationBuilder.AddColumn<DateTime>(
                name: "startWork",
                table: "DBTask",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "supervisorCod",
                table: "DBStaff",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "startWork",
                table: "DBTask");

            migrationBuilder.DropColumn(
                name: "supervisorCod",
                table: "DBStaff");

            migrationBuilder.AddColumn<int>(
                name: "supervisorId",
                table: "DBStaff",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
