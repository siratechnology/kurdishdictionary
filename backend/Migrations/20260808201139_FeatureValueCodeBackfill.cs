using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class FeatureValueCodeBackfill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // FeatureValues.Code, for databases that missed it.
            //
            // Its original migration was lost to a stray `ef migrations remove`. The replacement was
            // added as a guarded ALTER inside OperationsRoom — which fixed every database created
            // AFTER that point, and no database created before it, because OperationsRoom had
            // already been applied there. Fresh databases were fine and the working one was broken:
            // the reverse of the usual failure, and just as invisible.
            //
            // Its own migration this time, so every database gets it exactly once. Guarded, so the
            // ones already carrying the column are untouched.
            migrationBuilder.Sql(@"
                IF COL_LENGTH('FeatureValues', 'Code') IS NULL
                    ALTER TABLE [FeatureValues] ADD [Code] nvarchar(32) NULL;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
