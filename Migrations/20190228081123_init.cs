using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ac_equipment",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    IsDelete = table.Column<int>(nullable: true, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    CreatedByUserGuid = table.Column<Guid>(nullable: true),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    AuctionHouse = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    IsSold = table.Column<int>(nullable: true, defaultValue: 0),
                    IsPurchase = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ProductionDate = table.Column<DateTime>(nullable: true),
                    WorkingTime = table.Column<DateTime>(nullable: true),
                    DealPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    DealPriceRMB = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Long = table.Column<int>(nullable: true),
                    Width = table.Column<int>(nullable: true),
                    Height = table.Column<int>(nullable: true),
                    Weight = table.Column<double>(nullable: true),
                    Volume = table.Column<decimal>(type: "decimal(18, 3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ac_equipment", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "st_user",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    IsDelete = table.Column<int>(nullable: true, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    CreatedByUserGuid = table.Column<Guid>(nullable: true),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    LoginName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    RealName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    IsLocked = table.Column<int>(nullable: true, defaultValue: 0),
                    Description = table.Column<string>(type: "nvarchar(800)", nullable: true),
                    UserRole = table.Column<int>(nullable: true, defaultValue: 0),
                    LastLoginIp = table.Column<string>(nullable: true),
                    LastLoginAt = table.Column<DateTime>(nullable: true),
                    AvatorPath = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_user", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "ac_photo",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    IsDelete = table.Column<int>(nullable: true, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    CreatedByUserGuid = table.Column<Guid>(nullable: true),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    StoreDir = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    OriginName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Ranking = table.Column<int>(nullable: true),
                    IsHome = table.Column<bool>(nullable: true, defaultValue: false),
                    ContentType = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FileSize = table.Column<int>(nullable: true),
                    EquipmentPhotoGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ac_photo", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_ac_photo_ac_equipment_EquipmentPhotoGuid",
                        column: x => x.EquipmentPhotoGuid,
                        principalTable: "ac_equipment",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "st_login_log",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    IsDelete = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    CreatedByUserGuid = table.Column<Guid>(nullable: true),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true),
                    UserGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_login_log", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_st_login_log_st_user_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "st_user",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ac_photo_EquipmentPhotoGuid",
                table: "ac_photo",
                column: "EquipmentPhotoGuid");

            migrationBuilder.CreateIndex(
                name: "IX_st_login_log_UserGuid",
                table: "st_login_log",
                column: "UserGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ac_photo");

            migrationBuilder.DropTable(
                name: "st_login_log");

            migrationBuilder.DropTable(
                name: "ac_equipment");

            migrationBuilder.DropTable(
                name: "st_user");
        }
    }
}
