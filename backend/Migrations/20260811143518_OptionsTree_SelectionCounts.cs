using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <summary>
    /// Per-axis selection counts, and the index change that makes more than one answer per axis
    /// expressible at all.
    ///
    /// The index goes from UNIQUE(SenseId, AxisId) to UNIQUE(SenseId, AxisId, ValueId). That is a
    /// LOOSENING — every row that satisfied the old constraint satisfies the new one — so it cannot
    /// fail on live data and needs no cleanup pass first.
    ///
    /// What keeps ناو unchanged is not the index, it is MaxSelections = 1 on every axis that already
    /// exists. AddColumn's defaultValue backfills that, and the explicit UPDATE below repeats it so
    /// the guarantee does not depend on how a future EF version chooses to render a default. Losing
    /// it would silently let a finished part of speech start accepting a second value.
    /// </summary>
    public partial class OptionsTree_SelectionCounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SenseFeatures_SenseId_AxisId",
                table: "SenseFeatures");

            migrationBuilder.AddColumn<int>(
                name: "MaxSelections",
                table: "FeatureAxes",
                type: "int",
                nullable: true,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "MinSelections",
                table: "FeatureAxes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Belt and braces on the one thing this migration must not get wrong. Every axis that
            // existed before today stays single-choice, and none of them becomes "required" by
            // acquiring a minimum — nothing below the part of speech may ever gate a save.
            migrationBuilder.Sql("UPDATE [FeatureAxes] SET [MaxSelections] = 1, [MinSelections] = 0;");

            migrationBuilder.CreateIndex(
                name: "IX_SenseFeatures_SenseId_AxisId_ValueId",
                table: "SenseFeatures",
                columns: new[] { "SenseId", "AxisId", "ValueId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Going back TIGHTENS the constraint, so anything a multi-select axis collected would
            // block the index. Keep the newest answer per axis and drop the rest rather than
            // failing the rollback halfway through with a half-applied schema.
            migrationBuilder.Sql(@"
                DELETE f
                FROM [SenseFeatures] f
                WHERE f.[IsDeleted] = 0
                  AND EXISTS (
                      SELECT 1 FROM [SenseFeatures] keep
                      WHERE keep.[IsDeleted] = 0
                        AND keep.[SenseId] = f.[SenseId]
                        AND keep.[AxisId]  = f.[AxisId]
                        AND keep.[Id]      > f.[Id]);");

            migrationBuilder.DropIndex(
                name: "IX_SenseFeatures_SenseId_AxisId_ValueId",
                table: "SenseFeatures");

            migrationBuilder.DropColumn(
                name: "MaxSelections",
                table: "FeatureAxes");

            migrationBuilder.DropColumn(
                name: "MinSelections",
                table: "FeatureAxes");

            migrationBuilder.CreateIndex(
                name: "IX_SenseFeatures_SenseId_AxisId",
                table: "SenseFeatures",
                columns: new[] { "SenseId", "AxisId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
