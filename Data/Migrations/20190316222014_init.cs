using System;
using Microsoft.EntityFrameworkCore.Metadata;
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
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
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
                    IsPurchase = table.Column<string>(type: "nvarchar(50)", nullable: true, defaultValue: "No"),
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
                    table.PrimaryKey("PK_ac_equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "st_roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "st_users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    RealName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(800)", nullable: true),
                    AvatorPath = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ac_photo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    IsDelete = table.Column<int>(nullable: true, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    CreatedByUserGuid = table.Column<Guid>(nullable: true),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    RequestPath = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    SavePath = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    OriginName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Ranking = table.Column<int>(nullable: true),
                    IsCover = table.Column<bool>(nullable: true, defaultValue: false),
                    ContentType = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FileSize = table.Column<long>(nullable: true),
                    EquipmentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ac_photo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ac_photo_ac_equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "ac_equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "st_role_claims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_role_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_st_role_claims_st_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "st_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "st_login_log",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDelete = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    CreatedByUserGuid = table.Column<Guid>(nullable: true),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_login_log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_st_login_log_st_users_UserId",
                        column: x => x.UserId,
                        principalTable: "st_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "st_user_claims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_user_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_st_user_claims_st_users_UserId",
                        column: x => x.UserId,
                        principalTable: "st_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "st_user_logins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_user_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_st_user_logins_st_users_UserId",
                        column: x => x.UserId,
                        principalTable: "st_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "st_user_roles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_user_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_st_user_roles_st_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "st_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_st_user_roles_st_users_UserId",
                        column: x => x.UserId,
                        principalTable: "st_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "st_user_tokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_user_tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_st_user_tokens_st_users_UserId",
                        column: x => x.UserId,
                        principalTable: "st_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ac_photo_EquipmentId",
                table: "ac_photo",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_st_login_log_UserId",
                table: "st_login_log",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_st_role_claims_RoleId",
                table: "st_role_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "st_roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_st_user_claims_UserId",
                table: "st_user_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_st_user_logins_UserId",
                table: "st_user_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_st_user_roles_RoleId",
                table: "st_user_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "st_users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "st_users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ac_photo");

            migrationBuilder.DropTable(
                name: "st_login_log");

            migrationBuilder.DropTable(
                name: "st_role_claims");

            migrationBuilder.DropTable(
                name: "st_user_claims");

            migrationBuilder.DropTable(
                name: "st_user_logins");

            migrationBuilder.DropTable(
                name: "st_user_roles");

            migrationBuilder.DropTable(
                name: "st_user_tokens");

            migrationBuilder.DropTable(
                name: "ac_equipment");

            migrationBuilder.DropTable(
                name: "st_roles");

            migrationBuilder.DropTable(
                name: "st_users");
        }
    }
}
