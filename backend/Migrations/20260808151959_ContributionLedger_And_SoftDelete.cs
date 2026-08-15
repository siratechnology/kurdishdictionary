using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ContributionLedger_And_SoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Remove the synthetic seed ──────────────────────────────────────
            // AppDbContext used to HasData() 1,001 words (Id 10000-11000), 6,006 relations and 3
            // food categories. EF scaffolded 7,010 DeleteData calls for this migration, each keyed
            // by primary key alone — on a database that never received the seed, those would delete
            // whatever real rows happen to sit at those ids.
            //
            // These statements are guarded by the seed's fingerprint instead, so they are a no-op
            // against a database that never had it, and safe against one that did:
            //   · words     — the exact CreatedAt stamped by HasData
            //   · categories— the exact seeded names, and only when nothing references them
            // Anything the guards do not match is real data and is left alone.
            migrationBuilder.Sql(@"
                DELETE FROM [RelatedWords]
                WHERE  [WordId] BETWEEN 10000 AND 11000
                   OR  [TargetWordId] BETWEEN 10000 AND 11000;

                DELETE FROM [WordMeans]       WHERE [WordId] BETWEEN 10000 AND 11000;
                DELETE FROM [WordCategories]  WHERE [WordId] BETWEEN 10000 AND 11000;
                DELETE FROM [WordSpeechPanes] WHERE [WordId] BETWEEN 10000 AND 11000;

                DELETE FROM [Words]
                WHERE  [Id] BETWEEN 10000 AND 11000
                  AND  [CreatedAt] = '2026-04-15T15:02:54.130';

                DELETE FROM [Categories]
                WHERE  [Id] IN (1, 2, 3)
                  AND  [Name] IN (N'خواردنی سەرەکی', N'میوە و سەوزە', N'کەلوپەل و بەهارات')
                  AND  NOT EXISTS (SELECT 1 FROM [WordCategories] wc WHERE wc.[CategoryId] = [Categories].[Id]);
            ");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "WordSpeechPanes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedByUserId",
                table: "WordSpeechPanes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WordSpeechPanes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Words",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedByUserId",
                table: "Words",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Words",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "WordMeans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedByUserId",
                table: "WordMeans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WordMeans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "WordCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedByUserId",
                table: "WordCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WordCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "RelatedWords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedByUserId",
                table: "RelatedWords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RelatedWords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedByUserId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ContributionEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SourceKind = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContributionEvents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_IsDeleted",
                table: "Words",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsDeleted",
                table: "Categories",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ContributionEvents_OccurredAt",
                table: "ContributionEvents",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_ContributionEvents_UserId_EventType",
                table: "ContributionEvents",
                columns: new[] { "UserId", "EventType" });

            migrationBuilder.CreateIndex(
                name: "IX_ContributionEvents_WordId_OccurredAt",
                table: "ContributionEvents",
                columns: new[] { "WordId", "OccurredAt" });

            // ── Append-only, enforced by the database ──────────────────────────
            // Not a convention, and not only the EF interceptor: an interceptor is bypassed by any
            // raw SQL, any migration, any second application, and by SSMS. INSTEAD OF means the
            // UPDATE/DELETE never reaches the table at all — the transaction dies with a message
            // that says why, which is what makes a contributor's credit actually permanent.
            migrationBuilder.Sql(@"
                CREATE TRIGGER [TR_ContributionEvent_NoUpdateDelete]
                ON [ContributionEvents]
                INSTEAD OF UPDATE, DELETE
                AS
                BEGIN
                    SET NOCOUNT ON;
                    THROW 50001,
                        'ContributionEvents is append-only. A contribution record is the evidence for a contributor''s credit in the published dictionary and cannot be edited or removed. To correct a mistake, append a new event.',
                        1;
                END;
            ");

            // ── Contribution stats read model ──────────────────────────────────
            // Derived ENTIRELY from the ledger. There is no counter column anywhere in the schema,
            // so these numbers cannot drift from the events they summarise — which is the whole
            // reason the دەستکاریکەران total and the dashboard card disagreed by 10 (پڕۆمپت ١٠).
            //
            // A view rather than a materialised table: the counts must be correct the instant an
            // event lands, and staleness here would show up as a teacher's name missing from the
            // credits for their own work.
            migrationBuilder.Sql(@"
                CREATE VIEW [vw_ContributionStats] AS
                SELECT
                    u.[Id]                                                   AS [UserId],
                    u.[UserName],
                    u.[FullName],
                    COUNT(CASE WHEN e.[EventType] = 1  THEN 1 END)           AS [WordsCreated],
                    COUNT(CASE WHEN e.[EventType] IN (12, 13) THEN 1 END)    AS [SensesClassified],
                    COUNT(CASE WHEN e.[EventType] IN (20, 21) THEN 1 END)    AS [FeaturesSet],
                    COUNT(CASE WHEN e.[EventType] = 30 THEN 1 END)           AS [RelationsAdded],
                    COUNT(CASE WHEN e.[EventType] = 40 THEN 1 END)           AS [FormsAdded],
                    COUNT(CASE WHEN e.[EventType] IN (51, 52) THEN 1 END)    AS [ReviewsDone],
                    CASE
                        WHEN COUNT(CASE WHEN e.[EventType] IN (51, 52) THEN 1 END) = 0 THEN 0.0
                        ELSE CAST(COUNT(CASE WHEN e.[EventType] = 51 THEN 1 END) AS float)
                             / COUNT(CASE WHEN e.[EventType] IN (51, 52) THEN 1 END)
                    END                                                      AS [ApprovalRate],
                    MIN(e.[OccurredAt])                                      AS [FirstContributionAt],
                    MAX(e.[OccurredAt])                                      AS [LastContributionAt]
                FROM [AspNetUsers] u
                LEFT JOIN [ContributionEvents] e ON e.[UserId] = u.[Id]
                GROUP BY u.[Id], u.[UserName], u.[FullName];
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // The seed is deliberately NOT restored. It was synthetic data that inflated every
            // count on the dashboard; putting it back would recreate the defect this migration
            // exists to remove. Down() undoes the schema, not the cleanup.

            migrationBuilder.Sql("DROP VIEW IF EXISTS [vw_ContributionStats];");

            // The trigger has to go before the table it guards: DROP TABLE on a table with an
            // INSTEAD OF trigger is fine, but dropping it explicitly keeps Down() readable and
            // survives a future change that keeps the table.
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS [TR_ContributionEvent_NoUpdateDelete];");

            migrationBuilder.DropTable(
                name: "ContributionEvents");

            migrationBuilder.DropIndex(
                name: "IX_Words_IsDeleted",
                table: "Words");

            migrationBuilder.DropIndex(
                name: "IX_Categories_IsDeleted",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "WordSpeechPanes");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "WordSpeechPanes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WordSpeechPanes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "WordMeans");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "WordMeans");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WordMeans");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "WordCategories");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "WordCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WordCategories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "RelatedWords");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "RelatedWords");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RelatedWords");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

        }
    }
}
