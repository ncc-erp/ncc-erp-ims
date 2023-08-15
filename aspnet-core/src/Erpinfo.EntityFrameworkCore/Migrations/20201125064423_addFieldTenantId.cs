using Microsoft.EntityFrameworkCore.Migrations;

namespace Erpinfo.Migrations
{
    public partial class addFieldTenantId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "UserLikeSubComments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "UserLikeOrDislikeBlogPosts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "UserLikeMainComments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "UserCalendars",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "UserBlogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "SystemNotifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "SubComments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "PostHashtags",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Policies",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "News",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "MainComments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "HashTags",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GuildLines",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "EntityType",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "EntityGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "EntityChangeLog",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "BlogPosts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "UserLikeSubComments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "UserLikeOrDislikeBlogPosts");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "UserLikeMainComments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "UserCalendars");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "UserBlogs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SystemNotifications");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SubComments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PostHashtags");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "MainComments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "HashTags");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GuildLines");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "EntityGroups");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "EntityChangeLog");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "BlogPosts");
        }
    }
}
