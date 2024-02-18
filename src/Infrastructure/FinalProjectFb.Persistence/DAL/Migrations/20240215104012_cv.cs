using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectFb.Persistence.Dal.Migrations
{
    public partial class cv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cvs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Birthday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinnishCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cvs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_Address",
                table: "Cvs",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_FatherName",
                table: "Cvs",
                column: "FatherName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_FinnishCode",
                table: "Cvs",
                column: "FinnishCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_Name",
                table: "Cvs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_Surname",
                table: "Cvs",
                column: "Surname",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cvs");
        }
    }
}
