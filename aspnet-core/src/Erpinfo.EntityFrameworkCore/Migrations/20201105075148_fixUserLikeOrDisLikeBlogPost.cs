using Microsoft.EntityFrameworkCore.Migrations;

namespace Erpinfo.Migrations
{
    public partial class fixUserLikeOrDisLikeBlogPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIDReact",
                table: "UserLikeOrDislikeBlogPosts");

            migrationBuilder.AddColumn<long>(
                name: "UserID",
                table: "UserLikeOrDislikeBlogPosts",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "UserLikeOrDislikeBlogPosts");

            migrationBuilder.AddColumn<string>(
                name: "UserIDReact",
                table: "UserLikeOrDislikeBlogPosts",
                type: "varchar(max)",
                nullable: true);
        }
    }
}
