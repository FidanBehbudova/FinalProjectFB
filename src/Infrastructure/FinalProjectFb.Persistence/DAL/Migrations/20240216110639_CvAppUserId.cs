using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectFb.Persistence.Dal.Migrations
{
    public partial class CvAppUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Cvs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_AppUserId",
                table: "Cvs",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cvs_AspNetUsers_AppUserId",
                table: "Cvs",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cvs_AspNetUsers_AppUserId",
                table: "Cvs");

            migrationBuilder.DropIndex(
                name: "IX_Cvs_AppUserId",
                table: "Cvs");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Cvs");
        }
    }
}
