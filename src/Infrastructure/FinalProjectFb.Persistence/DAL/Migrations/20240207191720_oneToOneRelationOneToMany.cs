using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectFb.Persistence.Dal.Migrations
{
    public partial class oneToOneRelationOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_CompanyId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_JobId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyCities",
                table: "CompanyCities");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CompanyCities",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CompanyCities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CompanyCities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CompanyCities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "CompanyCities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyCities",
                table: "CompanyCities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CompanyId",
                table: "Images",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_JobId",
                table: "Images",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCities_CompanyId",
                table: "CompanyCities",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_CompanyId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_JobId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyCities",
                table: "CompanyCities");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCities_CompanyId",
                table: "CompanyCities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompanyCities");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CompanyCities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CompanyCities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CompanyCities");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "CompanyCities");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyCities",
                table: "CompanyCities",
                columns: new[] { "CompanyId", "CityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CompanyId",
                table: "Images",
                column: "CompanyId",
                unique: true,
                filter: "[CompanyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Images_JobId",
                table: "Images",
                column: "JobId",
                unique: true,
                filter: "[JobId] IS NOT NULL");
        }
    }
}
