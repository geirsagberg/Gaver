using Microsoft.EntityFrameworkCore.Migrations;

namespace Gaver.Web.Migrations;

public partial class Convert_Identity : Migration
{

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("UPDATE \"WishLists\" SET \"WishesOrder\" = '' WHERE \"WishesOrder\" IS NULL");
        migrationBuilder.AlterColumn<string>(
            name: "WishesOrder",
            table: "WishLists",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "WishesOrder",
            table: "WishLists",
            type: "text",
            nullable: true,
            oldClrType: typeof(string));
    }
}
