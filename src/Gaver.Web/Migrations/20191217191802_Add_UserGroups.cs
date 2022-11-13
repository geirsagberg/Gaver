using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Gaver.Web.Migrations;

public partial class Add_UserGroups : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserGroups",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                CreatedByUserId = table.Column<int>(nullable: false),
                Name = table.Column<string>(maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserGroups", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserGroups_Users_CreatedByUserId",
                    column: x => x.CreatedByUserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserGroupConnections",
            columns: table => new
            {
                UserId = table.Column<int>(nullable: false),
                UserGroupId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserGroupConnections", x => new { x.UserGroupId, x.UserId });
                table.ForeignKey(
                    name: "FK_UserGroupConnections_UserGroups_UserGroupId",
                    column: x => x.UserGroupId,
                    principalTable: "UserGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserGroupConnections_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UserGroupConnections_UserId",
            table: "UserGroupConnections",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserGroups_CreatedByUserId",
            table: "UserGroups",
            column: "CreatedByUserId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserGroupConnections");

        migrationBuilder.DropTable(
            name: "UserGroups");
    }
}
