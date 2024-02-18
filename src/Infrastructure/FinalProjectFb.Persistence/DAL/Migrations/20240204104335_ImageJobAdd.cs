using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectFb.Persistence.Dal.Migrations
{
    public partial class ImageJobAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Images_ImageId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ImageId",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_JobId",
                table: "Images",
                column: "JobId",
                unique: true,
                filter: "[JobId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Jobs_JobId",
                table: "Images",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Jobs_JobId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_JobId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Images");

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
    }
}
