using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Migrations
{
    public partial class equipmentAddPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCover",
                table: "ac_photo",
                newName: "IsHiddenAfterSold");

            migrationBuilder.AddColumn<Guid>(
                name: "BoomEquipmentId",
                table: "ac_photo",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CabEquipmentId",
                table: "ac_photo",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CoverEquipmentId",
                table: "ac_photo",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EngineEquipmentId",
                table: "ac_photo",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExteriorEquipmentId",
                table: "ac_photo",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrackedChassisEquipmentId",
                table: "ac_photo",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ac_photo_BoomEquipmentId",
                table: "ac_photo",
                column: "BoomEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ac_photo_CabEquipmentId",
                table: "ac_photo",
                column: "CabEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ac_photo_CoverEquipmentId",
                table: "ac_photo",
                column: "CoverEquipmentId",
                unique: true,
                filter: "[CoverEquipmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ac_photo_EngineEquipmentId",
                table: "ac_photo",
                column: "EngineEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ac_photo_ExteriorEquipmentId",
                table: "ac_photo",
                column: "ExteriorEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ac_photo_TrackedChassisEquipmentId",
                table: "ac_photo",
                column: "TrackedChassisEquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ac_photo_ac_equipment_BoomEquipmentId",
                table: "ac_photo",
                column: "BoomEquipmentId",
                principalTable: "ac_equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ac_photo_ac_equipment_CabEquipmentId",
                table: "ac_photo",
                column: "CabEquipmentId",
                principalTable: "ac_equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ac_photo_ac_equipment_CoverEquipmentId",
                table: "ac_photo",
                column: "CoverEquipmentId",
                principalTable: "ac_equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ac_photo_ac_equipment_EngineEquipmentId",
                table: "ac_photo",
                column: "EngineEquipmentId",
                principalTable: "ac_equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ac_photo_ac_equipment_ExteriorEquipmentId",
                table: "ac_photo",
                column: "ExteriorEquipmentId",
                principalTable: "ac_equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ac_photo_ac_equipment_TrackedChassisEquipmentId",
                table: "ac_photo",
                column: "TrackedChassisEquipmentId",
                principalTable: "ac_equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ac_photo_ac_equipment_BoomEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_ac_photo_ac_equipment_CabEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_ac_photo_ac_equipment_CoverEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_ac_photo_ac_equipment_EngineEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_ac_photo_ac_equipment_ExteriorEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropForeignKey(
                name: "FK_ac_photo_ac_equipment_TrackedChassisEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropIndex(
                name: "IX_ac_photo_BoomEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropIndex(
                name: "IX_ac_photo_CabEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropIndex(
                name: "IX_ac_photo_CoverEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropIndex(
                name: "IX_ac_photo_EngineEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropIndex(
                name: "IX_ac_photo_ExteriorEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropIndex(
                name: "IX_ac_photo_TrackedChassisEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropColumn(
                name: "BoomEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropColumn(
                name: "CabEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropColumn(
                name: "CoverEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropColumn(
                name: "EngineEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropColumn(
                name: "ExteriorEquipmentId",
                table: "ac_photo");

            migrationBuilder.DropColumn(
                name: "TrackedChassisEquipmentId",
                table: "ac_photo");

            migrationBuilder.RenameColumn(
                name: "IsHiddenAfterSold",
                table: "ac_photo",
                newName: "IsCover");
        }
    }
}
