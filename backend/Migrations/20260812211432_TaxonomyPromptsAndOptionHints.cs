using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <summary>
    /// Three additive columns, and a one-time suggestion pass over the dropdowns that already exist.
    ///
    /// Nothing is restructured: PartOfSpeechAxis.RequiresValueId is still the only parent link and no
    /// table is added or moved. What was missing was somewhere to put the SENTENCE a teacher reads,
    /// which the label column could not hold without making the admin's shorthand and the teacher's
    /// instruction the same string.
    /// </summary>
    public partial class TaxonomyPromptsAndOptionHints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OptionHintKu",
                table: "FeatureValues",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromptKu",
                table: "FeatureAxes",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PromptNeedsReview",
                table: "FeatureAxes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            SuggestPrompts(migrationBuilder);
        }

        /// <summary>
        /// Starting-point questions for the dropdowns the team has already built, every one flagged
        /// <c>PromptNeedsReview</c>.
        ///
        /// They are SUGGESTIONS and the flag says so on screen. The wording of a grammatical question
        /// is a linguistic judgement and this is not the place it gets made — the settings screen
        /// lists every flagged prompt so the team reads and corrects them. Anything not confidently
        /// known is deliberately absent rather than guessed at: a dropdown with no question falls back
        /// to its label, which reads as unfinished and gets asked about, whereas an invented question
        /// reads as finished and gets answered.
        ///
        /// Why this lives in the migration and not in <c>DbSeeder</c>: a seeder runs on every boot, so
        /// a question the team deliberately DELETED would come back on the next restart. A migration
        /// runs exactly once, which is the only correct number of times to offer a suggestion.
        ///
        /// Matching folds the Arabic/Kurdish letter pairs the live data mixes (ي/ی, ك/ک, ڕ/ر) the same
        /// way <c>KurdishText.Normalize</c> does, because these axis names were typed by hand on
        /// whichever keyboard was to hand and an unfolded comparison would silently match none of them.
        ///
        /// It matches on CONTAINMENT rather than equality, because the team named several of these
        /// dropdowns as phrases — «لە ڕووی ناسراوییەوە», not «ناسراوی». An exact comparison found four
        /// of them and silently skipped the rest, which is the worst outcome available: a half-done
        /// pass looks like a finished one.
        /// </summary>
        private static void SuggestPrompts(MigrationBuilder migrationBuilder)
        {
            // Fragment of the axis label (already folded) → the suggested question.
            //
            // «تایبەتمەندی» and «هەبوون» are ABSENT on purpose. The brief pairs them as one dropdown
            // carrying one compound question, and in the live data they are two separate dropdowns —
            // so splitting that question across them would be inventing linguistic phrasing, which is
            // the one thing this pass must not do. They stay null, fall back to their labels, and the
            // settings screen lists them as still needing a question.
            (string Fragment, string Prompt)[] suggestions =
            {
                ("رەگەز",   "ڕەگەزی وشەکە چییە؟"),
                ("ژمارە",   "تاکە یان کۆ؟"),
                ("رۆنان",   "پێکهاتەی وشەکە چۆنە؟"),
                ("ناسراوی", "ناسراوە یان نەناسراو؟"),
                ("جۆری کار", "کارەکە چ جۆرێکە؟"),
                ("تێپەری",  "ئایا کارەکە تێپەڕە یان تێنەپەڕ؟"),
            };

            foreach (var (fragment, prompt) in suggestions)
            {
                // Only ever fills a NULL. A prompt somebody has already written is never touched.
                //
                // ESCAPE N'\': a fragment is a literal, so an underscore or bracket in a future one
                // must not quietly become a LIKE wildcard and match half the table.
                migrationBuilder.Sql($"""
                    UPDATE FeatureAxes
                    SET    PromptKu = N'{prompt.Replace("'", "''")}',
                           PromptNeedsReview = 1
                    WHERE  PromptKu IS NULL
                      AND  REPLACE(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(NameKu)),
                               N'ي', N'ی'), N'ى', N'ی'), N'ك', N'ک'), N'ڕ', N'ر')
                           LIKE N'%{fragment.Replace("'", "''")}%' ESCAPE N'\';
                    """);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionHintKu",
                table: "FeatureValues");

            migrationBuilder.DropColumn(
                name: "PromptKu",
                table: "FeatureAxes");

            migrationBuilder.DropColumn(
                name: "PromptNeedsReview",
                table: "FeatureAxes");
        }
    }
}
