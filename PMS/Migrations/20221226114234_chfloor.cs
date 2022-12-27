using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class chfloor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "floorId",
                schema: "Identity",
                table: "tbl_RentalsDetails");

            migrationBuilder.AddColumn<string>(
                name: "floor",
                schema: "Identity",
                table: "tbl_RentalsDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "floor",
                schema: "Identity",
                table: "tbl_RentalsDetails");

            migrationBuilder.AddColumn<int>(
                name: "floorId",
                schema: "Identity",
                table: "tbl_RentalsDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
