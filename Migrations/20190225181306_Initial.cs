using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ac_equipment",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    IsDelete = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    CreatedByUserGuid = table.Column<Guid>(nullable: false),
                    CreatedByUserName = table.Column<string>(nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    AuctionHouse = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    IsSold = table.Column<int>(nullable: false),
                    IsPurchase = table.Column<string>(nullable: true),
                    ProductionDate = table.Column<DateTime>(nullable: false),
                    WorkingTime = table.Column<DateTime>(nullable: false),
                    DealPrice = table.Column<decimal>(nullable: false),
                    DealPriceRMB = table.Column<decimal>(nullable: false),
                    Long = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Volume = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ac_equipment", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "ac_photo",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    IsDelete = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    CreatedByUserGuid = table.Column<Guid>(nullable: false),
                    CreatedByUserName = table.Column<string>(nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(nullable: true),
                    AttachmentId = table.Column<int>(nullable: false),
                    AttachmentType = table.Column<string>(nullable: true),
                    StoreDir = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    OriginName = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    IsHome = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ac_photo", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "ac_equipment_photo",
                columns: table => new
                {
                    EquipmentGuid = table.Column<Guid>(nullable: false),
                    PhotoGuid = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ac_equipment_photo", x => new { x.EquipmentGuid, x.PhotoGuid });
                    table.ForeignKey(
                        name: "FK_ac_equipment_photo_ac_equipment_EquipmentGuid",
                        column: x => x.EquipmentGuid,
                        principalTable: "ac_equipment",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ac_equipment_photo_ac_photo_PhotoGuid",
                        column: x => x.PhotoGuid,
                        principalTable: "ac_photo",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ac_user",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    IsDelete = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    CreatedByUserGuid = table.Column<Guid>(nullable: false),
                    CreatedByUserName = table.Column<string>(nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(nullable: true),
                    LoginName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    AvatorGuid = table.Column<Guid>(nullable: true),
                    RealName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    IsLocked = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "nvarchar(800)", nullable: true),
                    UserRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ac_user", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_ac_user_ac_photo_AvatorGuid",
                        column: x => x.AvatorGuid,
                        principalTable: "ac_photo",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ac_equipment_photo_PhotoGuid",
                table: "ac_equipment_photo",
                column: "PhotoGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ac_user_AvatorGuid",
                table: "ac_user",
                column: "AvatorGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ac_equipment_photo");

            migrationBuilder.DropTable(
                name: "ac_user");

            migrationBuilder.DropTable(
                name: "ac_equipment");

            migrationBuilder.DropTable(
                name: "ac_photo");
        }
    }
}
