using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectFb.Persistence.Dal.Migrations
{
    public partial class CvJobIdAndJobListCv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Cvs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_JobId",
                table: "Cvs",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cvs_Jobs_JobId",
                table: "Cvs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cvs_Jobs_JobId",
                table: "Cvs");

            migrationBuilder.DropIndex(
                name: "IX_Cvs_JobId",
                table: "Cvs");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Cvs");
        }
    }
}
