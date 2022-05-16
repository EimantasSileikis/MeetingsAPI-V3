using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingsAPI_V3.Migrations
{
    public partial class AddedStringForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Users",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Users",
                table: "Meetings");
        }
    }
}
