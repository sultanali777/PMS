using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class enarGov : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Identity",
                table: "tbl_Governorates");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_Governorates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_Governorates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_Governorates");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_Governorates");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "tbl_Governorates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
