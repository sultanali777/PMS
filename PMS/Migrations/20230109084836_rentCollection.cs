using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class rentCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_rentCollection",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userIdReceived = table.Column<string>(nullable: true),
                    rentalId = table.Column<int>(nullable: false),
                    propertyRent = table.Column<int>(nullable: false),
                    monthRent = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    receivedType = table.Column<string>(nullable: true),
                    receivedDate = table.Column<DateTime>(nullable: false),
                    receivedRent = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rentCollection", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_rentCollection",
                schema: "Identity");
        }
    }
}
