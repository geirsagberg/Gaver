using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gaver.Web.Migrations;

public partial class Hide_UserFriendConnections : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Invitations");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Invitations",
            columns: table => new
            {
                WishListId = table.Column<int>(type: "integer", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Invitations", x => new { x.WishListId, x.UserId });
                table.ForeignKey(
                    name: "FK_Invitations_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Invitations_WishLists_WishListId",
                    column: x => x.WishListId,
                    principalTable: "WishLists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Invitations_UserId",
            table: "Invitations",
            column: "UserId");
    }
}
