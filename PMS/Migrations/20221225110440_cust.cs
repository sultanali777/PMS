using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class cust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CivilIdNo",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CivilIdNo",
                schema: "Identity",
                table: "tbl_Customer",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
