using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Migrations
{
    public partial class useraddcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "st_login_log");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "ac_photo");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "ac_equipment");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "st_users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsDeleted",
                table: "st_users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "st_users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsDeleted",
                table: "st_login_log",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsDeleted",
                table: "ac_photo",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsDeleted",
                table: "ac_equipment",
                nullable: true,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "st_users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "st_users");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "st_users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "st_login_log");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ac_photo");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ac_equipment");

            migrationBuilder.AddColumn<int>(
                name: "IsDelete",
                table: "st_login_log",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsDelete",
                table: "ac_photo",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsDelete",
                table: "ac_equipment",
                nullable: true,
                defaultValue: 0);
        }
    }
}
