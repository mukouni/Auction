using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Migrations
{
    public partial class addEquipmentRemarkAndDistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RBCode",
                table: "ac_equipment",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "ac_equipment",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkingDistance",
                table: "ac_equipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkingDistanceUnit",
                table: "ac_equipment",
                type: "nvarchar(20)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RBCode",
                table: "ac_equipment");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "ac_equipment");

            migrationBuilder.DropColumn(
                name: "WorkingDistance",
                table: "ac_equipment");

            migrationBuilder.DropColumn(
                name: "WorkingDistanceUnit",
                table: "ac_equipment");
        }
    }
}
