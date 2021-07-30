using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddingGridLayoutTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GridLayoutMasters",
                columns: table => new
                {
                    GridLayoutMasterID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LayoutName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoofColumns = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridLayoutMasters", x => x.GridLayoutMasterID);
                });

            migrationBuilder.CreateTable(
                name: "GridLayoutDetails",
                columns: table => new
                {
                    GridLayoutDetailID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GridLayoutMasterID = table.Column<long>(type: "bigint", nullable: false),
                    CameraIP = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridLayoutDetails", x => x.GridLayoutDetailID);
                    table.ForeignKey(
                        name: "FK_GridLayoutDetails_GridLayoutMasters_GridLayoutMasterID",
                        column: x => x.GridLayoutMasterID,
                        principalTable: "GridLayoutMasters",
                        principalColumn: "GridLayoutMasterID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GridLayoutDetails_GridLayoutMasterID",
                table: "GridLayoutDetails",
                column: "GridLayoutMasterID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GridLayoutDetails");

            migrationBuilder.DropTable(
                name: "GridLayoutMasters");
        }
    }
}
