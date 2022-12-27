using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class prochnge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "floorId",
                schema: "Identity",
                table: "tbl_Property");

            migrationBuilder.AddColumn<string>(
                name: "floor",
                schema: "Identity",
                table: "tbl_Property",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "floor",
                schema: "Identity",
                table: "tbl_Property");

            migrationBuilder.AddColumn<int>(
                name: "floorId",
                schema: "Identity",
                table: "tbl_Property",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
