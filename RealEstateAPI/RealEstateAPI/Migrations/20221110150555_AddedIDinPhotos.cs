using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAPI.Migrations
{
    public partial class AddedIDinPhotos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Photos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "PublicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");
        }
    }
}
