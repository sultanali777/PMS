using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class newChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "passportNo",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.AddColumn<string>(
                name: "attachments",
                schema: "Identity",
                table: "tbl_Vendor",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "advanceAmount",
                schema: "Identity",
                table: "tbl_RentalsDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "aaliNo",
                schema: "Identity",
                table: "tbl_Property",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "legalCost",
                schema: "Identity",
                table: "tbl_Property",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "aaliNo",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "business",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "companyName",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guranAaliNo",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guranAddress",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "guranCivilIdNo",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "guranMobileNo",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guranfullName",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attachments",
                schema: "Identity",
                table: "tbl_Vendor");

            migrationBuilder.DropColumn(
                name: "advanceAmount",
                schema: "Identity",
                table: "tbl_RentalsDetails");

            migrationBuilder.DropColumn(
                name: "aaliNo",
                schema: "Identity",
                table: "tbl_Property");

            migrationBuilder.DropColumn(
                name: "legalCost",
                schema: "Identity",
                table: "tbl_Property");

            migrationBuilder.DropColumn(
                name: "aaliNo",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.DropColumn(
                name: "business",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.DropColumn(
                name: "companyName",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.DropColumn(
                name: "guranAaliNo",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.DropColumn(
                name: "guranAddress",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.DropColumn(
                name: "guranCivilIdNo",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.DropColumn(
                name: "guranMobileNo",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.DropColumn(
                name: "guranfullName",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.AddColumn<string>(
                name: "passportNo",
                schema: "Identity",
                table: "tbl_Customer",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
