using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class init_pravki_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "DBStaff");

            migrationBuilder.UpdateData(
                table: "DBProject",
                keyColumn: "supervisor",
                keyValue: "Дегтярёв М.Н.",
                column: "supervisor",
                value: "Дегтярёв М.Н");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
