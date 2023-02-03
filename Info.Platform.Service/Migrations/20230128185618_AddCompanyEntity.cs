using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Info.PlatformService.Migrations
{
    public partial class AddCompanyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Platforms");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Platforms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Platforms_CompanyId",
                table: "Platforms",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Platforms_Companies_CompanyId",
                table: "Platforms",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Platforms_Companies_CompanyId",
                table: "Platforms");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Platforms_CompanyId",
                table: "Platforms");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Platforms");

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Platforms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
