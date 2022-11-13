using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Gaver.Web.Migrations;

public partial class Add_InvitationToken : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "InvitationTokens",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                Token = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "NOW()"),
                WishListId = table.Column<int>(nullable: false),
                Accepted = table.Column<DateTimeOffset>(nullable: true)
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
            name: "IX_InvitationTokens_WishListId",
            table: "InvitationTokens",
            column: "WishListId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "InvitationTokens");
    }
}
