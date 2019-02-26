using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ac_equipment_photo_ac_equipment_EquipmentGuid",
                table: "ac_equipment_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_ac_equipment_photo_ac_photo_PhotoGuid",
                table: "ac_equipment_photo");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "ac_photo",
                newName: "Ranking");

            migrationBuilder.AddForeignKey(
                name: "FK_ac_equipment_photo_ac_equipment_EquipmentGuid",
                table: "ac_equipment_photo",
                column: "EquipmentGuid",
                principalTable: "ac_equipment",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ac_equipment_photo_ac_photo_PhotoGuid",
                table: "ac_equipment_photo",
                column: "PhotoGuid",
                principalTable: "ac_photo",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ac_equipment_photo_ac_equipment_EquipmentGuid",
                table: "ac_equipment_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_ac_equipment_photo_ac_photo_PhotoGuid",
                table: "ac_equipment_photo");

            migrationBuilder.RenameColumn(
                name: "Ranking",
                table: "ac_photo",
                newName: "Order");

            migrationBuilder.AddForeignKey(
                name: "FK_ac_equipment_photo_ac_equipment_EquipmentGuid",
                table: "ac_equipment_photo",
                column: "EquipmentGuid",
                principalTable: "ac_equipment",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ac_equipment_photo_ac_photo_PhotoGuid",
                table: "ac_equipment_photo",
                column: "PhotoGuid",
                principalTable: "ac_photo",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
