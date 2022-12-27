using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMS.Migrations
{
    public partial class tbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Areas",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    governorateId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_AttachmentDetails",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(nullable: true),
                    attachmentTypeId = table.Column<int>(nullable: false),
                    attachmentPath = table.Column<string>(nullable: true),
                    date_Created = table.Column<DateTime>(nullable: false)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AttachmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Building",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(nullable: true),
                    buildingno = table.Column<string>(nullable: true),
                    governorateId = table.Column<int>(nullable: false),
                    groundfloor = table.Column<bool>(nullable: false),
                    basement = table.Column<bool>(nullable: false),
                    m1 = table.Column<bool>(nullable: false),
                    m2 = table.Column<bool>(nullable: false),
                    m3 = table.Column<bool>(nullable: false),
                    areaId = table.Column<int>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    date_Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Building", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Customer",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(nullable: true),
                    fullName = table.Column<string>(nullable: true),
                    mobileNo = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    governorateId = table.Column<int>(nullable: false),
                    areaId = table.Column<int>(nullable: false),
                    CivilIdNo = table.Column<int>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    passportNo = table.Column<string>(nullable: true),
                    nationalityId = table.Column<int>(nullable: false),
                    date_Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ExpenseDetails",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(nullable: true),
                    buildingId = table.Column<int>(nullable: false),
                    floorId = table.Column<int>(nullable: false),
                    propertyTypeId = table.Column<int>(nullable: false),
                    propertyNo = table.Column<int>(nullable: false),
                    expenseAmount = table.Column<int>(nullable: false),
                    invoiceNo = table.Column<string>(nullable: true),
                    vendorId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    date_Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ExpenseDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Governorates",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Property",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(nullable: true),
                    buildingId = table.Column<int>(nullable: false),
                    floorId = table.Column<int>(nullable: false),
                    propertyTypeId = table.Column<int>(nullable: false),
                    propertyNo = table.Column<int>(nullable: false),
                    basicRent = table.Column<int>(nullable: false),
                    statusId = table.Column<int>(nullable: false),
                    date_Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Property", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_PropertyType",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_PropertyType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_RentalsDetails",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(nullable: true),
                    buildingId = table.Column<int>(nullable: false),
                    floorId = table.Column<int>(nullable: false),
                    propertyTypeId = table.Column<int>(nullable: false),
                    propertyNo = table.Column<int>(nullable: false),
                    propertyRent = table.Column<int>(nullable: false),
                    startDate = table.Column<DateTime>(nullable: false),
                    endDate = table.Column<DateTime>(nullable: false),
                    customerId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    date_Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RentalsDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Vendor",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(nullable: true),
                    fullName = table.Column<string>(nullable: true),
                    companyName = table.Column<string>(nullable: true),
                    mobileNo = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    governorateId = table.Column<int>(nullable: false),
                    areaId = table.Column<int>(nullable: false),
                    CivilIdNo = table.Column<int>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    vendorTypeId = table.Column<int>(nullable: false),
                    date_Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_VendorType",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_VendorType", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Areas",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_AttachmentDetails",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_AttachmentType",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Building",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Customer",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_ExpenseDetails",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Governorates",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Property",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_PropertyType",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_RentalsDetails",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_Vendor",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "tbl_VendorType",
                schema: "Identity");
        }
    }
}
