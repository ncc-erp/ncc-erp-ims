using Microsoft.EntityFrameworkCore.Migrations;

namespace Erpinfo.Migrations
{
    public partial class Update_Table_AbpUsers_Add_column_KomuUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "KomuUserId",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KomuUserId",
                table: "AbpUsers");
        }
    }
}
