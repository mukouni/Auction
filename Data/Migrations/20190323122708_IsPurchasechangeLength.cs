using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Migrations
{
    public partial class IsPurchasechangeLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsPurchase",
                table: "ac_equipment",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldDefaultValue: "No");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IsPurchase",
                table: "ac_equipment",
                type: "nvarchar(50)",
                nullable: true,
                defaultValue: "No",
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 0);
        }
    }
}
