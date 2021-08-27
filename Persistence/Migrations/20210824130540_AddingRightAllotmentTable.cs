using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddingRightAllotmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RightsAllotmentHistories",
                columns: table => new
                {
                    RightsAllotmentHID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RightsAllotmentMID = table.Column<long>(type: "bigint", nullable: false),
                    RightID = table.Column<int>(type: "int", nullable: false),
                    EnteredUserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserSystem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RightsAllotmentHistories", x => x.RightsAllotmentHID);
                });

            migrationBuilder.CreateTable(
                name: "RightsAllotmentMs",
                columns: table => new
                {
                    RightsAllotmentID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    EntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignByUserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    AssignBySystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignByIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateUserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdateSystemIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateSystemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsCancel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RightsAllotmentMs", x => x.RightsAllotmentID);
                });

            migrationBuilder.CreateTable(
                name: "RightsAllotmentDs",
                columns: table => new
                {
                    RightsAllotmentDID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RightsAllotmentMID = table.Column<long>(type: "bigint", nullable: false),
                    RightID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RightsAllotmentDs", x => x.RightsAllotmentDID);
                    table.ForeignKey(
                        name: "FK_RightsAllotmentDs_RightsAllotmentMs_RightsAllotmentMID",
                        column: x => x.RightsAllotmentMID,
                        principalTable: "RightsAllotmentMs",
                        principalColumn: "RightsAllotmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RightsAllotmentDs_RightsAllotmentMID",
                table: "RightsAllotmentDs",
                column: "RightsAllotmentMID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RightsAllotmentDs");

            migrationBuilder.DropTable(
                name: "RightsAllotmentHistories");

            migrationBuilder.DropTable(
                name: "RightsAllotmentMs");
        }
    }
}
