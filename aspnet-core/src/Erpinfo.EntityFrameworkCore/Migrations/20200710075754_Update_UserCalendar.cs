using Microsoft.EntityFrameworkCore.Migrations;

namespace Erpinfo.Migrations
{
    public partial class Update_UserCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "UserCalendars");

            migrationBuilder.AddColumn<string>(
                name: "Entity",
                table: "UserCalendars",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EntityId",
                table: "UserCalendars",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entity",
                table: "UserCalendars");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "UserCalendars");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "UserCalendars",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
