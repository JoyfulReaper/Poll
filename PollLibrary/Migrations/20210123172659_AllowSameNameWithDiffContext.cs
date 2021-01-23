using Microsoft.EntityFrameworkCore.Migrations;

namespace PollLibrary.Migrations
{
    public partial class AllowSameNameWithDiffContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Polls_Name",
                table: "Polls");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Polls_Name",
                table: "Polls",
                column: "Name",
                unique: true);
        }
    }
}
