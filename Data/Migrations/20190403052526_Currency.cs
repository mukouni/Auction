using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Migrations
{
    public partial class Currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DealPriceCurrencyId",
                table: "ac_equipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineNo",
                table: "ac_equipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrameNo",
                table: "ac_equipment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotNo",
                table: "ac_equipment",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ac_equipment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceCurrencyId",
                table: "ac_equipment",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "st_currency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SymbolCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ISOCode = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_currency", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ac_equipment_DealPriceCurrencyId",
                table: "ac_equipment",
                column: "DealPriceCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ac_equipment_PriceCurrencyId",
                table: "ac_equipment",
                column: "PriceCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ac_equipment_st_currency_DealPriceCurrencyId",
                table: "ac_equipment",
                column: "DealPriceCurrencyId",
                principalTable: "st_currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ac_equipment_st_currency_PriceCurrencyId",
                table: "ac_equipment",
                column: "PriceCurrencyId",
                principalTable: "st_currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ac_equipment_st_currency_DealPriceCurrencyId",
                table: "ac_equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_ac_equipment_st_currency_PriceCurrencyId",
                table: "ac_equipment");

            migrationBuilder.DropTable(
                name: "st_currency");

            migrationBuilder.DropIndex(
                name: "IX_ac_equipment_DealPriceCurrencyId",
                table: "ac_equipment");

            migrationBuilder.DropIndex(
                name: "IX_ac_equipment_PriceCurrencyId",
                table: "ac_equipment");

            migrationBuilder.DropColumn(
                name: "DealPriceCurrencyId",
                table: "ac_equipment");

            migrationBuilder.DropColumn(
                name: "EngineNo",
                table: "ac_equipment");

            migrationBuilder.DropColumn(
                name: "FrameNo",
                table: "ac_equipment");

            migrationBuilder.DropColumn(
                name: "LotNo",
                table: "ac_equipment");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ac_equipment");

            migrationBuilder.DropColumn(
                name: "PriceCurrencyId",
                table: "ac_equipment");
        }
    }
}
