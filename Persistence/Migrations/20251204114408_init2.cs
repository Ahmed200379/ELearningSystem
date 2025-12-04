using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialGroup");

            migrationBuilder.AddColumn<DateTime>(
                name: "AdditionDate",
                table: "Materials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Materials",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_GroupId",
                table: "Materials",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Groups_GroupId",
                table: "Materials",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Groups_GroupId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_GroupId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "AdditionDate",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Materials");

            migrationBuilder.CreateTable(
                name: "MaterialGroup",
                columns: table => new
                {
                    GroupsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaterialsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialGroup", x => new { x.GroupsId, x.MaterialsId });
                    table.ForeignKey(
                        name: "FK_MaterialGroup_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialGroup_Materials_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialGroup_MaterialsId",
                table: "MaterialGroup",
                column: "MaterialsId");
        }
    }
}
