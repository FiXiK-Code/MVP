using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class redact_roleCod_to_Staff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
              name: "DBStaff");

            migrationBuilder.CreateTable(
                name: "DBStaff",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    divisionId = table.Column<int>(nullable: false),
                    post = table.Column<string>(nullable: true),
                    roleId = table.Column<int>(nullable: false),
                    supervisorCod = table.Column<string>(nullable: true),
                    login = table.Column<string>(nullable: true),
                    passvord = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBStaff", x => x.id);
                });

            migrationBuilder.DropColumn(
                name: "roleId",
                table: "DBStaff");

            migrationBuilder.AddColumn<string>(
                name: "roleCod",
                table: "DBStaff",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mail",
                table: "DBStaff",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
