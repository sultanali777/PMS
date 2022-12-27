using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class tblchnge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "basement",
                schema: "Identity",
                table: "tbl_Building");

            migrationBuilder.DropColumn(
                name: "groundfloor",
                schema: "Identity",
                table: "tbl_Building");

            migrationBuilder.DropColumn(
                name: "m1",
                schema: "Identity",
                table: "tbl_Building");

            migrationBuilder.DropColumn(
                name: "m2",
                schema: "Identity",
                table: "tbl_Building");

            migrationBuilder.DropColumn(
                name: "m3",
                schema: "Identity",
                table: "tbl_Building");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Identity",
                table: "tbl_Areas");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_Areas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_Areas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_Areas");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_Areas");

            migrationBuilder.AddColumn<bool>(
                name: "basement",
                schema: "Identity",
                table: "tbl_Building",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "groundfloor",
                schema: "Identity",
                table: "tbl_Building",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "m1",
                schema: "Identity",
                table: "tbl_Building",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "m2",
                schema: "Identity",
                table: "tbl_Building",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "m3",
                schema: "Identity",
                table: "tbl_Building",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "tbl_Areas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
