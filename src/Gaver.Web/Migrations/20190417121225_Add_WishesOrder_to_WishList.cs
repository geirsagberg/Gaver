using Microsoft.EntityFrameworkCore.Migrations;

namespace Gaver.Web.Migrations
{
    public partial class Add_WishesOrder_to_WishList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WishesOrder",
                table: "WishLists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WishesOrder",
                table: "WishLists");
        }
    }
}
