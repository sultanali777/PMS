using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class enar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Identity",
                table: "tbl_VendorType");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Identity",
                table: "tbl_Status");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Identity",
                table: "tbl_PropertyType");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Identity",
                table: "tbl_Country");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_VendorType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_VendorType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_Status",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_Status",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_PropertyType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_PropertyType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_Country",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_Country",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_VendorType");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_VendorType");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_Status");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_Status");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_PropertyType");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_PropertyType");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                schema: "Identity",
                table: "tbl_Country");

            migrationBuilder.DropColumn(
                name: "EnglishName",
                schema: "Identity",
                table: "tbl_Country");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "tbl_VendorType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "tbl_Status",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "tbl_PropertyType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "tbl_Country",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
