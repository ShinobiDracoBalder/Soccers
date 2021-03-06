using Microsoft.EntityFrameworkCore.Migrations;

namespace Soccers.Web.Migrations
{
    public partial class TheBadMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tournaments_Name",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Groups_Name",
                table: "Groups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_Name",
                table: "Tournaments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name",
                table: "Groups",
                column: "Name",
                unique: true);
        }
    }
}
