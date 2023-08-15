using Microsoft.EntityFrameworkCore.Migrations;

namespace Erpinfo.Migrations
{
    public partial class Add_CoverImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Policies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "News",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "GuildLines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "News");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "GuildLines");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Events");
        }
    }
}
