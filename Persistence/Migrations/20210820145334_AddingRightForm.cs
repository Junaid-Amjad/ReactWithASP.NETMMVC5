using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddingRightForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rights",
                columns: table => new
                {
                    RightID = table.Column<int>(type: "int", nullable: false),
                    RightName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isCancel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rights", x => x.RightID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rights");
        }
    }
}
