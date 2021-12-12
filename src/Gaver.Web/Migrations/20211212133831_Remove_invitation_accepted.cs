using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gaver.Web.Migrations
{
    public partial class Remove_invitation_accepted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "InvitationTokens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Accepted",
                table: "InvitationTokens",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
