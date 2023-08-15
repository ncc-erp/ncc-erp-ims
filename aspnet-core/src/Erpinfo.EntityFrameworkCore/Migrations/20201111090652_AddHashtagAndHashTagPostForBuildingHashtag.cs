using Microsoft.EntityFrameworkCore.Migrations;

namespace Erpinfo.Migrations
{
    public partial class AddHashtagAndHashTagPostForBuildingHashtag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostHashtags",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPost = table.Column<long>(nullable: false),
                    IdHashTag = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostHashtags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostHashtags_HashTags_IdHashTag",
                        column: x => x.IdHashTag,
                        principalTable: "HashTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostHashtags_BlogPosts_IdPost",
                        column: x => x.IdPost,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostHashtags_IdHashTag",
                table: "PostHashtags",
                column: "IdHashTag");

            migrationBuilder.CreateIndex(
                name: "IX_PostHashtags_IdPost",
                table: "PostHashtags",
                column: "IdPost");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostHashtags");
        }
    }
}
