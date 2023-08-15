using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Erpinfo.Migrations
{
    public partial class Add_Table_Like_NewsEventsGuidelinePolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLikeOrDislikeEvents",
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
                    UserId = table.Column<long>(nullable: false),
                    EventId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikeOrDislikeEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLikeOrDislikeEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikeOrDislikeEvents_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLikeOrDislikeGuideLines",
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
                    UserId = table.Column<long>(nullable: false),
                    GuideLineId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikeOrDislikeGuideLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLikeOrDislikeGuideLines_GuildLines_GuideLineId",
                        column: x => x.GuideLineId,
                        principalTable: "GuildLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikeOrDislikeGuideLines_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLikeOrDislikeNews",
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
                    UserId = table.Column<long>(nullable: false),
                    NewsId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikeOrDislikeNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLikeOrDislikeNews_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikeOrDislikeNews_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLikeOrDislikePolicies",
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
                    UserId = table.Column<long>(nullable: false),
                    PoliciesId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikeOrDislikePolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLikeOrDislikePolicies_Policies_PoliciesId",
                        column: x => x.PoliciesId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikeOrDislikePolicies_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeOrDislikeEvents_EventId",
                table: "UserLikeOrDislikeEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeOrDislikeEvents_UserId",
                table: "UserLikeOrDislikeEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeOrDislikeGuideLines_GuideLineId",
                table: "UserLikeOrDislikeGuideLines",
                column: "GuideLineId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeOrDislikeGuideLines_UserId",
                table: "UserLikeOrDislikeGuideLines",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeOrDislikeNews_NewsId",
                table: "UserLikeOrDislikeNews",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeOrDislikeNews_UserId",
                table: "UserLikeOrDislikeNews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeOrDislikePolicies_PoliciesId",
                table: "UserLikeOrDislikePolicies",
                column: "PoliciesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeOrDislikePolicies_UserId",
                table: "UserLikeOrDislikePolicies",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikeOrDislikeEvents");

            migrationBuilder.DropTable(
                name: "UserLikeOrDislikeGuideLines");

            migrationBuilder.DropTable(
                name: "UserLikeOrDislikeNews");

            migrationBuilder.DropTable(
                name: "UserLikeOrDislikePolicies");
        }
    }
}
