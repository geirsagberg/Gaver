using Microsoft.EntityFrameworkCore.Migrations;

namespace Gaver.Web.Migrations;

public partial class Add_UserFriendConnection : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserFriendConnections",
            columns: table => new
            {
                UserId = table.Column<int>(nullable: false),
                FriendId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserFriendConnections", x => new { x.UserId, x.FriendId });
                table.ForeignKey(
                    name: "FK_UserFriendConnections_Users_FriendId",
                    column: x => x.FriendId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserFriendConnections_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UserFriendConnections_FriendId",
            table: "UserFriendConnections",
            column: "FriendId");

        migrationBuilder.Sql(@"
INSERT INTO ""UserFriendConnections"" (""UserId"", ""FriendId"")
SELECT i.""UserId"", wl.""UserId"" FROM ""Invitations"" i JOIN ""WishLists"" wl ON wl.""Id"" = i.""WishListId""
UNION
SELECT wl.""UserId"", i.""UserId"" FROM ""Invitations"" i JOIN ""WishLists"" wl ON wl.""Id"" = i.""WishListId""
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserFriendConnections");
    }
}
