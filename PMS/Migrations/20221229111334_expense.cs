using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class expense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "floorId",
                schema: "Identity",
                table: "tbl_ExpenseDetails");

            migrationBuilder.AddColumn<string>(
                name: "attachments",
                schema: "Identity",
                table: "tbl_ExpenseDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "floor",
                schema: "Identity",
                table: "tbl_ExpenseDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guid",
                schema: "Identity",
                table: "tbl_ExpenseDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attachments",
                schema: "Identity",
                table: "tbl_ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "floor",
                schema: "Identity",
                table: "tbl_ExpenseDetails");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "Identity",
                table: "tbl_ExpenseDetails");

            migrationBuilder.AddColumn<int>(
                name: "floorId",
                schema: "Identity",
                table: "tbl_ExpenseDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
