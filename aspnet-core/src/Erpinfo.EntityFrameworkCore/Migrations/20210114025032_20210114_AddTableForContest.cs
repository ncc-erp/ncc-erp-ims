using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Erpinfo.Migrations
{
    public partial class _20210114_AddTableForContest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContestImages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    StatusOfEventPublish = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ContestName = table.Column<string>(nullable: true),
                    MaxImagesPerDay = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImagePublishes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ImageURI = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    ContestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePublishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagePublishes_ContestImages_ContestId",
                        column: x => x.ContestId,
                        principalTable: "ContestImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImagePublishes_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLikePublishImages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    IsLiked = table.Column<bool>(nullable: false),
                    ImageId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikePublishImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLikePublishImages_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLikePublishImages_ImagePublishes_ImageId",
                        column: x => x.ImageId,
                        principalTable: "ImagePublishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagePublishes_ContestId",
                table: "ImagePublishes",
                column: "ContestId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePublishes_CreatorUserId",
                table: "ImagePublishes",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikePublishImages_CreatorUserId",
                table: "UserLikePublishImages",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikePublishImages_ImageId",
                table: "UserLikePublishImages",
                column: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikePublishImages");

            migrationBuilder.DropTable(
                name: "ImagePublishes");

            migrationBuilder.DropTable(
                name: "ContestImages");
        }
    }
}
