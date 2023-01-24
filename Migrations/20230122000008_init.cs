using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DBCompanyStructure",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    divisionsId = table.Column<int>(nullable: false),
                    supervisor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBCompanyStructure", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBDivision",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBDivision", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBLogistickProject",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    projectId = table.Column<int>(nullable: false),
                    arhive = table.Column<string>(nullable: true),
                    link = table.Column<string>(nullable: true),
                    supervisor = table.Column<string>(nullable: true),
                    priority = table.Column<int>(nullable: false),
                    allStages = table.Column<string>(nullable: true),
                    CommitorId = table.Column<int>(nullable: false),
                    dateRedaction = table.Column<DateTime>(nullable: false),
                    comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBLogistickProject", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBLogistickTask",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaskCode = table.Column<string>(nullable: true),
                    ProjectCode = table.Column<string>(nullable: true),
                    TaskId = table.Column<int>(nullable: false),
                    descTask = table.Column<string>(nullable: true),
                    supervisorId = table.Column<int>(nullable: false),
                    resipienId = table.Column<int>(nullable: false),
                    dateRedaction = table.Column<DateTime>(nullable: false),
                    planedTime = table.Column<TimeSpan>(nullable: false),
                    actualTime = table.Column<TimeSpan>(nullable: false),
                    CommitorId = table.Column<int>(nullable: false),
                    taskStatusId = table.Column<int>(nullable: false),
                    comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBLogistickTask", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBPost",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    roleCod = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBPost", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBProject",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    shortName = table.Column<string>(nullable: true),
                    priority = table.Column<int>(nullable: false),
                    dateStart = table.Column<DateTime>(nullable: false),
                    plannedFinishDate = table.Column<DateTime>(nullable: false),
                    actualFinishDate = table.Column<DateTime>(nullable: false),
                    supervisor = table.Column<string>(nullable: true),
                    link = table.Column<string>(nullable: true),
                    history = table.Column<string>(nullable: true),
                    archive = table.Column<string>(nullable: true),
                    nowStage = table.Column<string>(nullable: true),
                    allStages = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBProject", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBRole",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    supervisor = table.Column<string>(nullable: true),
                    recipient = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBRole", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "DBStage",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    stageId = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    projectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBStage", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBTask",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(nullable: true),
                    desc = table.Column<string>(nullable: true),
                    TaskCodeParent = table.Column<string>(nullable: true),
                    projectCode = table.Column<string>(nullable: true),
                    supervisor = table.Column<string>(nullable: true),
                    recipient = table.Column<string>(nullable: true),
                    priority = table.Column<int>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    plannedTime = table.Column<TimeSpan>(nullable: false),
                    actualTime = table.Column<TimeSpan>(nullable: false),
                    start = table.Column<DateTime>(nullable: false),
                    finish = table.Column<DateTime>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    Stage = table.Column<string>(nullable: true),
                    liteTask = table.Column<bool>(nullable: false),
                    status = table.Column<string>(nullable: true),
                    startWork = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBTask", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBTaskStatus",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBTaskStatus", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DBCompanyStructure");

            migrationBuilder.DropTable(
                name: "DBDivision");

            migrationBuilder.DropTable(
                name: "DBLogistickProject");

            migrationBuilder.DropTable(
                name: "DBLogistickTask");

            migrationBuilder.DropTable(
                name: "DBPost");

            migrationBuilder.DropTable(
                name: "DBProject");

            migrationBuilder.DropTable(
                name: "DBRole");

            migrationBuilder.DropTable(
                name: "DBStaff");

            migrationBuilder.DropTable(
                name: "DBStage");

            migrationBuilder.DropTable(
                name: "DBTask");

            migrationBuilder.DropTable(
                name: "DBTaskStatus");
        }
    }
}
