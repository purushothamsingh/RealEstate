using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAPI.Migrations
{
    public partial class mig9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FurnishingTypeId",
                table: "Wishes");

            migrationBuilder.RenameColumn(
                name: "PropertyTypeId",
                table: "Wishes",
                newName: "PropertyId");

            migrationBuilder.AddColumn<string>(
                name: "FurnishingType",
                table: "Wishes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                table: "Wishes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FurnishingType",
                table: "Wishes");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Wishes");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "Wishes",
                newName: "PropertyTypeId");

            migrationBuilder.AddColumn<int>(
                name: "FurnishingTypeId",
                table: "Wishes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
