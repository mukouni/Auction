using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Migrations
{
    public partial class removeLogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "st_login_log");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "st_login_log",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedByUserGuid = table.Column<Guid>(nullable: true),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_st_login_log_UserId",
                table: "st_login_log",
                column: "UserId");
        }
    }
}
