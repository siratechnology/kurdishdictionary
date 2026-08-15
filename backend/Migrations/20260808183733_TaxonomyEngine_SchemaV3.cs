using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class TaxonomyEngine_SchemaV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Normalized",
                table: "Words",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameKu = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Domains_Domains_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Domains",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FeatureAxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    NameKu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AllowsNotApplicable = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureAxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartsOfSpeech",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    NameKu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsOfSpeech", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    NameKu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Scope = table.Column<int>(type: "int", nullable: false),
                    IsSymmetric = table.Column<bool>(type: "bit", nullable: false),
                    InverseId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelationTypes_RelationTypes_InverseId",
                        column: x => x.InverseId,
                        principalTable: "RelationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WordFormTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameKu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordFormTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AxisId = table.Column<int>(type: "int", nullable: false),
                    NameKu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MergedIntoValueId = table.Column<int>(type: "int", nullable: true),
                    MergedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureValues_FeatureAxes_AxisId",
                        column: x => x.AxisId,
                        principalTable: "FeatureAxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeatureValues_FeatureValues_MergedIntoValueId",
                        column: x => x.MergedIntoValueId,
                        principalTable: "FeatureValues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Senses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PartOfSpeechId = table.Column<int>(type: "int", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: true),
                    ExampleUsage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    WorkflowState = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Completeness = table.Column<int>(type: "int", nullable: false, computedColumnSql: "(CASE WHEN LEN(LTRIM(RTRIM([Definition]))) > 0 THEN 34 ELSE 0 END +  CASE WHEN LEN(LTRIM(RTRIM([ExampleUsage]))) > 0 THEN 33 ELSE 0 END +  CASE WHEN [DomainId] IS NOT NULL THEN 33 ELSE 0 END)", stored: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Senses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Senses_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Senses_PartsOfSpeech_PartOfSpeechId",
                        column: x => x.PartOfSpeechId,
                        principalTable: "PartsOfSpeech",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Senses_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromWordId = table.Column<int>(type: "int", nullable: false),
                    ToWordId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    IsAutoInverse = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordRelations_RelationTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "RelationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WordRelations_Words_FromWordId",
                        column: x => x.FromWordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordRelations_Words_ToWordId",
                        column: x => x.ToWordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WordForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    Form = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FormTypeId = table.Column<int>(type: "int", nullable: false),
                    Normalized = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordForms_WordFormTypes_FormTypeId",
                        column: x => x.FormTypeId,
                        principalTable: "WordFormTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WordForms_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartOfSpeechAxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartOfSpeechId = table.Column<int>(type: "int", nullable: false),
                    AxisId = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    RequiresValueId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartOfSpeechAxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartOfSpeechAxes_FeatureAxes_AxisId",
                        column: x => x.AxisId,
                        principalTable: "FeatureAxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartOfSpeechAxes_FeatureValues_RequiresValueId",
                        column: x => x.RequiresValueId,
                        principalTable: "FeatureValues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartOfSpeechAxes_PartsOfSpeech_PartOfSpeechId",
                        column: x => x.PartOfSpeechId,
                        principalTable: "PartsOfSpeech",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SenseFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenseId = table.Column<int>(type: "int", nullable: false),
                    AxisId = table.Column<int>(type: "int", nullable: false),
                    ValueId = table.Column<int>(type: "int", nullable: true),
                    IsNotApplicable = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenseFeatures", x => x.Id);
                    table.CheckConstraint("CK_SenseFeature_ValueOrNotApplicable", "([ValueId] IS NOT NULL AND [IsNotApplicable] = 0) OR ([ValueId] IS NULL AND [IsNotApplicable] = 1)");
                    table.ForeignKey(
                        name: "FK_SenseFeatures_FeatureAxes_AxisId",
                        column: x => x.AxisId,
                        principalTable: "FeatureAxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SenseFeatures_FeatureValues_ValueId",
                        column: x => x.ValueId,
                        principalTable: "FeatureValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SenseFeatures_Senses_SenseId",
                        column: x => x.SenseId,
                        principalTable: "Senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SenseRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromSenseId = table.Column<int>(type: "int", nullable: false),
                    ToSenseId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    IsAutoInverse = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenseRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SenseRelations_RelationTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "RelationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SenseRelations_Senses_FromSenseId",
                        column: x => x.FromSenseId,
                        principalTable: "Senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SenseRelations_Senses_ToSenseId",
                        column: x => x.ToSenseId,
                        principalTable: "Senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SenseTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenseId = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    OriginalLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenseTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SenseTranslations_Senses_SenseId",
                        column: x => x.SenseId,
                        principalTable: "Senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PartsOfSpeech",
                columns: new[] { "Id", "Code", "DeletedAt", "DeletedByUserId", "Description", "IsActive", "IsDeleted", "NameKu", "SortOrder" },
                values: new object[,]
                {
                    { 1, "noun", null, null, null, true, false, "ناو", 1 },
                    { 2, "verb", null, null, null, true, false, "کار", 2 },
                    { 3, "adjective", null, null, null, true, false, "هاوەڵناو", 3 },
                    { 4, "adverb", null, null, null, true, false, "هاوەڵکار", 4 },
                    { 5, "pronoun", null, null, null, true, false, "جێناو", 5 },
                    { 6, "particle", null, null, null, true, false, "ئامڕاز", 6 },
                    { 7, "infinitive", null, null, null, true, false, "چاوگ", 7 }
                });

            migrationBuilder.InsertData(
                table: "RelationTypes",
                columns: new[] { "Id", "Code", "InverseId", "IsActive", "IsSymmetric", "NameKu", "Scope", "SortOrder" },
                values: new object[,]
                {
                    { 1, "root", null, true, false, "ڕەگ", 0, 1 },
                    { 2, "derived-from", null, true, false, "داڕێژراو لێی", 0, 2 },
                    { 3, "component", null, true, false, "پێکهاتە", 0, 3 },
                    { 4, "part-of", null, true, false, "بەشێکە لە", 0, 4 },
                    { 5, "infinitive-of", null, true, false, "چاوگی کارەکە", 0, 5 },
                    { 6, "verb-of", null, true, false, "کاری چاوگەکە", 0, 6 },
                    { 7, "regional", null, true, true, "زاراوەی هەرێمی", 0, 7 },
                    { 8, "synonym", null, true, true, "هاومانا", 1, 8 },
                    { 9, "antonym", null, true, true, "پێچەوانە", 1, 9 },
                    { 10, "broader", null, true, false, "مانای گشتیتر", 1, 10 },
                    { 11, "narrower", null, true, false, "مانای وردتر", 1, 11 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_Normalized",
                table: "Words",
                column: "Normalized");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_ParentId_NameKu",
                table: "Domains",
                columns: new[] { "ParentId", "NameKu" },
                unique: true,
                filter: "[ParentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureAxes_Code",
                table: "FeatureAxes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureValues_AxisId_NameKu",
                table: "FeatureValues",
                columns: new[] { "AxisId", "NameKu" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureValues_MergedIntoValueId",
                table: "FeatureValues",
                column: "MergedIntoValueId");

            migrationBuilder.CreateIndex(
                name: "IX_PartOfSpeechAxes_AxisId",
                table: "PartOfSpeechAxes",
                column: "AxisId");

            migrationBuilder.CreateIndex(
                name: "IX_PartOfSpeechAxes_PartOfSpeechId_AxisId",
                table: "PartOfSpeechAxes",
                columns: new[] { "PartOfSpeechId", "AxisId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartOfSpeechAxes_RequiresValueId",
                table: "PartOfSpeechAxes",
                column: "RequiresValueId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsOfSpeech_Code",
                table: "PartsOfSpeech",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartsOfSpeech_SortOrder",
                table: "PartsOfSpeech",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_RelationTypes_Code",
                table: "RelationTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelationTypes_InverseId",
                table: "RelationTypes",
                column: "InverseId");

            migrationBuilder.CreateIndex(
                name: "IX_SenseFeatures_AxisId",
                table: "SenseFeatures",
                column: "AxisId");

            migrationBuilder.CreateIndex(
                name: "IX_SenseFeatures_SenseId_AxisId",
                table: "SenseFeatures",
                columns: new[] { "SenseId", "AxisId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_SenseFeatures_ValueId",
                table: "SenseFeatures",
                column: "ValueId");

            migrationBuilder.CreateIndex(
                name: "IX_SenseRelations_FromSenseId_ToSenseId_TypeId",
                table: "SenseRelations",
                columns: new[] { "FromSenseId", "ToSenseId", "TypeId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_SenseRelations_ToSenseId",
                table: "SenseRelations",
                column: "ToSenseId");

            migrationBuilder.CreateIndex(
                name: "IX_SenseRelations_TypeId",
                table: "SenseRelations",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Senses_DomainId",
                table: "Senses",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Senses_PartOfSpeechId",
                table: "Senses",
                column: "PartOfSpeechId");

            migrationBuilder.CreateIndex(
                name: "IX_Senses_WordId",
                table: "Senses",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_Senses_WorkflowState",
                table: "Senses",
                column: "WorkflowState");

            migrationBuilder.CreateIndex(
                name: "IX_SenseTranslations_SenseId_LanguageCode",
                table: "SenseTranslations",
                columns: new[] { "SenseId", "LanguageCode" });

            migrationBuilder.CreateIndex(
                name: "IX_WordForms_FormTypeId",
                table: "WordForms",
                column: "FormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WordForms_Normalized",
                table: "WordForms",
                column: "Normalized");

            migrationBuilder.CreateIndex(
                name: "IX_WordForms_WordId",
                table: "WordForms",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_FromWordId_ToWordId_TypeId",
                table: "WordRelations",
                columns: new[] { "FromWordId", "ToWordId", "TypeId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_ToWordId",
                table: "WordRelations",
                column: "ToWordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordRelations_TypeId",
                table: "WordRelations",
                column: "TypeId");

            // ── Relation inverse wiring ────────────────────────────────────────
            // Applied here rather than in HasData because the pairs reference each other
            // (ڕەگ ↔ داڕێژراو لێی) and EF topologically sorts seed inserts by foreign key — a
            // two-row cycle has no valid insert order and the scaffolder fails outright. Both rows
            // exist by this point, so the column is simply set afterwards.
            //
            // Symmetric types (زاراوەی هەرێمی، هاومانا، پێچەوانە) keep a NULL inverse: they are
            // their own inverse, and giving them one would make the auto-inverse logic in پڕۆمپت ٦
            // write a second identical edge.
            migrationBuilder.Sql(backend.Data.TaxonomyModelConfiguration.InverseWiringSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartOfSpeechAxes");

            migrationBuilder.DropTable(
                name: "SenseFeatures");

            migrationBuilder.DropTable(
                name: "SenseRelations");

            migrationBuilder.DropTable(
                name: "SenseTranslations");

            migrationBuilder.DropTable(
                name: "WordForms");

            migrationBuilder.DropTable(
                name: "WordRelations");

            migrationBuilder.DropTable(
                name: "FeatureValues");

            migrationBuilder.DropTable(
                name: "Senses");

            migrationBuilder.DropTable(
                name: "WordFormTypes");

            migrationBuilder.DropTable(
                name: "RelationTypes");

            migrationBuilder.DropTable(
                name: "FeatureAxes");

            migrationBuilder.DropTable(
                name: "Domains");

            migrationBuilder.DropTable(
                name: "PartsOfSpeech");

            migrationBuilder.DropIndex(
                name: "IX_Words_Normalized",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Normalized",
                table: "Words");
        }
    }
}
