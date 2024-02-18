using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectFb.Persistence.Dal.Migrations
{
    public partial class JobImageAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ImageId",
                table: "Jobs",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Images_ImageId",
                table: "Jobs",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Images_ImageId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ImageId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Jobs");
        }
    }
}
