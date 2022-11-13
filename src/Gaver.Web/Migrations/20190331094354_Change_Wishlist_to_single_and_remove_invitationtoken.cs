using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gaver.Web.Migrations;

public partial class Change_Wishlist_to_single_and_remove_invitationtoken : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "InvitationTokens");

        migrationBuilder.DropIndex(
            name: "IX_WishLists_UserId",
            table: "WishLists");

        migrationBuilder.CreateIndex(
            name: "IX_WishLists_UserId",
            table: "WishLists",
            column: "UserId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_WishLists_UserId",
            table: "WishLists");

        migrationBuilder.CreateTable(
            name: "InvitationTokens",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Accepted = table.Column<DateTimeOffset>(nullable: true),
                Created = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "NOW()"),
                WishListId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InvitationTokens", x => x.Id);
                table.ForeignKey(
                    name: "FK_InvitationTokens_WishLists_WishListId",
                    column: x => x.WishListId,
                    principalTable: "WishLists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_WishLists_UserId",
            table: "WishLists",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_InvitationTokens_WishListId",
            table: "InvitationTokens",
            column: "WishListId");
    }
}
