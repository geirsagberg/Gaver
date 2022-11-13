using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Gaver.Web.Migrations;

public partial class Add_WishOption : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "WishOptions",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                WishId = table.Column<int>(nullable: false),
                Title = table.Column<string>(maxLength: 255, nullable: false),
                Url = table.Column<string>(maxLength: 255, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WishOptions", x => x.Id);
                table.ForeignKey(
                    name: "FK_WishOptions_Wishes_WishId",
                    column: x => x.WishId,
                    principalTable: "Wishes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_WishOptions_WishId",
            table: "WishOptions",
            column: "WishId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "WishOptions");
    }
}
