using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "guid",
                schema: "Identity",
                table: "tbl_RentalsDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guid",
                schema: "Identity",
                table: "tbl_RentalsDetails");
        }
    }
}
