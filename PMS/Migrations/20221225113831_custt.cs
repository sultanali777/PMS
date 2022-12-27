using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class custt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_AttachmentDetails",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_AttachmentType",
                schema: "Identity");

            migrationBuilder.AddColumn<string>(
                name: "attachments",
                schema: "Identity",
                table: "tbl_RentalsDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "attachments",
                schema: "Identity",
                table: "tbl_Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attachments",
                schema: "Identity",
                table: "tbl_RentalsDetails");

            migrationBuilder.DropColumn(
                name: "attachments",
                schema: "Identity",
                table: "tbl_Customer");

            migrationBuilder.CreateTable(
                name: "tbl_AttachmentDetails",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    attachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    attachmentTypeId = table.Column<int>(type: "int", nullable: false),
                    date_Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AttachmentDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_AttachmentType",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AttachmentType", x => x.Id);
                });
        }
    }
}
