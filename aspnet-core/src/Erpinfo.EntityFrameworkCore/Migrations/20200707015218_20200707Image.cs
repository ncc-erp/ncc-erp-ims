using Microsoft.EntityFrameworkCore.Migrations;

namespace Erpinfo.Migrations
{
    public partial class _20200707Image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Policies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "News",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "GuildLines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "GuildLines");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Events");
        }
    }
}
