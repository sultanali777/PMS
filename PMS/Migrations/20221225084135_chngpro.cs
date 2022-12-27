using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class chngpro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "basicRent",
                schema: "Identity",
                table: "tbl_Property");

            migrationBuilder.AlterColumn<string>(
                name: "propertyNo",
                schema: "Identity",
                table: "tbl_Property",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "propertyNo",
                schema: "Identity",
                table: "tbl_Property",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "basicRent",
                schema: "Identity",
                table: "tbl_Property",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
