using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class build : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "buildingName",
                schema: "Identity",
                table: "tbl_Building",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerAddress",
                schema: "Identity",
                table: "tbl_Building",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerMobile",
                schema: "Identity",
                table: "tbl_Building",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerName",
                schema: "Identity",
                table: "tbl_Building",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "buildingName",
                schema: "Identity",
                table: "tbl_Building");

            migrationBuilder.DropColumn(
                name: "ownerAddress",
                schema: "Identity",
                table: "tbl_Building");

            migrationBuilder.DropColumn(
                name: "ownerMobile",
                schema: "Identity",
                table: "tbl_Building");

            migrationBuilder.DropColumn(
                name: "ownerName",
                schema: "Identity",
                table: "tbl_Building");
        }
    }
}
