using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDictionarySection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DictionarySectionId",
                table: "Words",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DictionarySections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameKu = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Normalized = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionarySections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_DictionarySectionId",
                table: "Words",
                column: "DictionarySectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionarySections_Normalized",
                table: "DictionarySections",
                column: "Normalized",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_DictionarySections_DictionarySectionId",
                table: "Words",
                column: "DictionarySectionId",
                principalTable: "DictionarySections",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_DictionarySections_DictionarySectionId",
                table: "Words");

            migrationBuilder.DropTable(
                name: "DictionarySections");

            migrationBuilder.DropIndex(
                name: "IX_Words_DictionarySectionId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "DictionarySectionId",
                table: "Words");
        }
    }
}
