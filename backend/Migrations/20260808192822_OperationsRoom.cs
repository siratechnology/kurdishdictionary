using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class OperationsRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── FeatureValues.Code ─────────────────────────────────────────────
            // This belongs to پڕۆمپت ٦ (rules key on a stable code, never on a renameable label).
            // Its own migration was lost to an `ef migrations remove` that took the wrong one: the
            // column stayed in the model snapshot, so `migrations add` saw no difference and never
            // regenerated it — the local database had the column and every fresh one did not.
            //
            // Guarded rather than a plain AddColumn so it is a no-op where the lost migration had
            // already run, and creates the column everywhere else.
            migrationBuilder.Sql(@"
                IF COL_LENGTH('FeatureValues', 'Code') IS NULL
                    ALTER TABLE [FeatureValues] ADD [Code] nvarchar(32) NULL;
            ");

            migrationBuilder.AddColumn<int>(
                name: "TrustLevel",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ConsistencySamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenseId = table.Column<int>(type: "int", nullable: false),
                    SampledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsistencySamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsistencySamples_Senses_SenseId",
                        column: x => x.SenseId,
                        principalTable: "Senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SenseClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleasedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenseClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SenseClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SenseClaims_Senses_SenseId",
                        column: x => x.SenseId,
                        principalTable: "Senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SenseDisagreements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenseId = table.Column<int>(type: "int", nullable: false),
                    AxisId = table.Column<int>(type: "int", nullable: true),
                    FirstJudgement = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FirstUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SecondJudgement = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SecondUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecondNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Resolution = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenseDisagreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SenseDisagreements_AspNetUsers_FirstUserId",
                        column: x => x.FirstUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SenseDisagreements_AspNetUsers_SecondUserId",
                        column: x => x.SecondUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SenseDisagreements_FeatureAxes_AxisId",
                        column: x => x.AxisId,
                        principalTable: "FeatureAxes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SenseDisagreements_Senses_SenseId",
                        column: x => x.SenseId,
                        principalTable: "Senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsistencySamples_ReadAt_SampledAt",
                table: "ConsistencySamples",
                columns: new[] { "ReadAt", "SampledAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ConsistencySamples_SenseId",
                table: "ConsistencySamples",
                column: "SenseId");

            migrationBuilder.CreateIndex(
                name: "IX_SenseClaims_SenseId",
                table: "SenseClaims",
                column: "SenseId",
                unique: true,
                filter: "[ReleasedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SenseClaims_UserId_ExpiresAt",
                table: "SenseClaims",
                columns: new[] { "UserId", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SenseDisagreements_AxisId",
                table: "SenseDisagreements",
                column: "AxisId");

            migrationBuilder.CreateIndex(
                name: "IX_SenseDisagreements_FirstUserId",
                table: "SenseDisagreements",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SenseDisagreements_SecondUserId",
                table: "SenseDisagreements",
                column: "SecondUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SenseDisagreements_SenseId_ResolvedAt",
                table: "SenseDisagreements",
                columns: new[] { "SenseId", "ResolvedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsistencySamples");

            migrationBuilder.DropTable(
                name: "SenseClaims");

            migrationBuilder.DropTable(
                name: "SenseDisagreements");

            migrationBuilder.DropColumn(
                name: "TrustLevel",
                table: "AspNetUsers");
        }
    }
}
