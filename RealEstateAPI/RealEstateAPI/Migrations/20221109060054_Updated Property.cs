﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAPI.Migrations
{
    public partial class UpdatedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Photos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
