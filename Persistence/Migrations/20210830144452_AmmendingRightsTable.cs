using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AmmendingRightsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TotalRightValue",
                table: "RightsAllotmentMs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalRightValue",
                table: "RightsAllotmentHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RightValue",
                table: "Rights",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalRightValue",
                table: "RightsAllotmentMs");

            migrationBuilder.DropColumn(
                name: "TotalRightValue",
                table: "RightsAllotmentHistories");

            migrationBuilder.DropColumn(
                name: "RightValue",
                table: "Rights");
        }
    }
}
