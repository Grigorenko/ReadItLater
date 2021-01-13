using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadItLater.Data.EF.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Refs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FolderId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    Url = table.Column<string>(maxLength: 500, nullable: false),
                    Image = table.Column<string>(maxLength: 500, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refs_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    RefId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Refs_RefId",
                        column: x => x.RefId,
                        principalTable: "Refs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TagRefs",
                columns: table => new
                {
                    TagId = table.Column<Guid>(nullable: false),
                    RefId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagRefs", x => new { x.RefId, x.TagId });
                    table.ForeignKey(
                        name: "FK_TagRefs_Refs_RefId",
                        column: x => x.RefId,
                        principalTable: "Refs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagRefs_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_Id",
                table: "Folders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentId",
                table: "Folders",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Refs_FolderId",
                table: "Refs",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Refs_Id",
                table: "Refs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagRefs_TagId",
                table: "TagRefs",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TagRefs_RefId_TagId",
                table: "TagRefs",
                columns: new[] { "RefId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Id",
                table: "Tags",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_RefId",
                table: "Tags",
                column: "RefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagRefs");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Refs");

            migrationBuilder.DropTable(
                name: "Folders");
        }
    }
}
