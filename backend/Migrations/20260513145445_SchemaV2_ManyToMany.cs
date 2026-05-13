using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SchemaV2_ManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "SpeechPane",
                table: "Words");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordSpeechPanes",
                columns: table => new
                {
                    WordId = table.Column<int>(type: "int", nullable: false),
                    SpeechPaneType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordSpeechPanes", x => new { x.WordId, x.SpeechPaneType });
                    table.ForeignKey(
                        name: "FK_WordSpeechPanes_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordCategories",
                columns: table => new
                {
                    WordId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordCategories", x => new { x.WordId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_WordCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordCategories_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "خواردنی سەرەکی" },
                    { 2, "میوە و سەوزە" },
                    { 3, "کەلوپەل و بەهارات" }
                });










































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordCategories_CategoryId",
                table: "WordCategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordCategories");

            migrationBuilder.DropTable(
                name: "WordSpeechPanes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Words");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Words",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpeechPane",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 14);

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10000,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10001,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10002,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10003,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10004,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10005,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10006,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10007,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10008,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10009,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10010,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10011,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10012,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10013,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10014,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10015,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10016,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10017,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10018,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10019,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10020,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10021,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10022,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10023,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10024,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10025,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10026,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10027,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10028,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10029,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10030,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10031,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10032,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10033,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10034,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10035,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10036,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10037,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10038,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10039,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10040,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10041,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10042,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10043,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10044,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10045,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10046,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10047,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10048,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10049,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10050,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10051,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10052,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10053,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10054,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10055,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10056,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10057,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10058,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10059,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10060,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10061,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10062,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10063,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10064,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10065,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10066,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10067,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10068,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10069,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10070,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10071,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10072,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10073,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10074,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10075,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10076,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10077,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10078,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10079,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10080,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10081,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10082,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10083,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10084,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10085,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10086,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10087,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10088,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10089,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10090,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10091,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10092,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10093,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10094,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10095,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10096,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10097,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10098,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10099,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10100,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10101,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10102,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10103,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10104,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10105,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10106,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10107,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10108,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10109,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10110,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10111,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10112,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10113,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10114,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10115,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10116,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10117,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10118,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10119,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10120,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10121,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10122,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10123,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10124,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10125,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10126,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10127,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10128,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10129,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10130,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10131,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10132,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10133,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10134,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10135,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10136,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10137,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10138,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10139,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10140,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10141,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10142,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10143,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10144,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10145,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10146,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10147,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10148,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10149,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10150,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10151,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10152,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10153,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10154,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10155,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10156,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10157,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10158,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10159,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10160,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10161,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10162,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10163,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10164,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10165,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10166,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10167,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10168,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10169,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10170,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10171,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10172,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10173,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10174,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10175,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10176,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10177,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10178,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10179,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10180,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10181,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10182,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10183,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10184,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10185,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10186,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10187,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10188,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10189,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10190,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10191,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10192,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10193,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10194,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10195,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10196,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10197,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10198,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10199,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10200,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10201,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10202,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10203,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10204,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10205,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10206,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10207,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10208,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10209,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10210,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10211,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10212,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10213,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10214,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10215,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10216,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10217,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10218,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10219,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10220,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10221,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10222,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10223,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10224,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10225,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10226,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10227,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10228,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10229,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10230,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10231,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10232,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10233,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10234,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10235,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10236,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10237,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10238,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10239,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10240,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10241,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10242,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10243,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10244,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10245,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10246,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10247,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10248,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10249,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10250,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10251,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10252,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10253,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10254,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10255,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10256,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10257,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10258,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10259,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10260,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10261,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10262,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10263,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10264,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10265,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10266,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10267,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10268,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10269,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10270,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10271,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10272,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10273,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10274,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10275,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10276,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10277,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10278,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10279,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10280,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10281,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10282,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10283,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10284,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10285,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10286,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10287,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10288,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10289,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10290,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10291,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10292,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10293,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10294,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10295,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10296,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10297,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10298,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10299,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10300,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10301,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10302,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10303,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10304,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10305,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10306,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10307,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10308,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10309,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10310,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10311,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10312,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10313,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10314,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10315,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10316,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10317,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10318,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10319,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10320,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10321,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10322,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10323,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10324,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10325,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10326,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10327,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10328,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10329,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10330,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10331,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10332,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10333,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10334,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10335,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10336,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10337,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10338,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10339,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10340,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10341,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10342,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10343,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10344,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10345,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10346,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10347,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10348,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10349,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10350,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10351,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10352,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10353,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10354,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10355,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10356,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10357,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10358,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10359,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10360,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10361,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10362,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10363,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10364,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10365,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10366,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10367,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10368,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10369,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10370,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10371,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10372,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10373,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10374,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10375,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10376,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10377,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10378,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10379,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10380,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10381,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10382,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10383,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10384,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10385,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10386,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10387,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10388,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10389,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10390,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10391,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10392,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10393,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10394,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10395,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10396,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10397,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10398,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10399,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10400,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10401,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10402,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10403,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10404,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10405,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10406,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10407,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10408,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10409,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10410,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10411,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10412,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10413,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10414,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10415,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10416,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10417,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10418,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10419,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10420,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10421,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10422,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10423,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10424,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10425,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10426,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10427,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10428,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10429,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10430,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10431,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10432,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10433,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10434,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10435,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10436,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10437,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10438,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10439,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10440,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10441,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10442,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10443,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10444,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10445,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10446,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10447,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10448,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10449,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10450,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10451,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10452,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10453,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10454,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10455,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10456,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10457,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10458,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10459,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10460,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10461,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10462,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10463,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10464,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10465,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10466,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10467,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10468,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10469,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10470,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10471,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10472,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10473,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10474,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10475,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10476,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10477,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10478,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10479,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10480,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10481,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10482,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10483,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10484,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10485,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10486,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10487,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10488,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10489,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10490,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10491,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10492,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10493,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10494,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10495,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10496,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10497,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10498,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10499,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10500,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10501,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10502,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10503,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10504,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10505,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10506,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10507,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10508,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10509,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10510,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10511,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10512,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10513,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10514,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10515,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10516,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10517,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10518,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10519,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10520,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10521,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10522,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10523,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10524,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10525,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10526,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10527,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10528,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10529,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10530,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10531,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10532,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10533,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10534,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10535,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10536,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10537,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10538,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10539,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10540,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10541,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10542,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10543,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10544,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10545,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10546,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10547,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10548,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10549,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10550,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10551,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10552,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10553,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10554,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10555,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10556,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10557,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10558,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10559,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10560,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10561,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10562,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10563,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10564,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10565,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10566,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10567,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10568,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10569,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10570,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10571,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10572,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10573,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10574,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10575,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10576,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10577,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10578,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10579,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10580,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10581,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10582,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10583,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10584,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10585,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10586,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10587,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10588,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10589,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10590,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10591,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10592,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10593,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10594,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10595,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10596,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10597,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10598,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10599,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10600,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10601,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10602,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10603,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10604,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10605,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10606,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10607,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10608,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10609,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10610,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10611,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10612,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10613,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10614,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10615,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10616,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10617,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10618,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10619,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10620,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10621,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10622,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10623,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10624,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10625,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10626,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10627,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10628,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10629,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10630,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10631,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10632,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10633,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10634,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10635,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10636,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10637,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10638,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10639,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10640,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10641,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10642,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10643,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10644,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10645,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10646,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10647,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10648,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10649,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10650,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10651,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10652,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10653,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10654,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10655,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10656,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10657,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10658,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10659,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10660,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10661,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10662,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10663,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10664,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10665,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10666,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10667,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10668,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10669,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10670,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10671,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10672,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10673,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10674,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10675,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10676,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10677,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10678,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10679,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10680,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10681,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10682,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10683,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10684,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10685,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10686,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10687,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10688,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10689,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10690,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10691,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10692,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10693,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10694,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10695,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10696,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10697,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10698,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10699,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10700,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10701,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10702,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10703,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10704,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10705,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10706,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10707,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10708,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10709,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10710,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10711,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10712,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10713,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10714,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10715,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10716,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10717,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10718,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10719,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10720,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10721,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10722,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10723,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10724,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10725,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10726,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10727,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10728,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10729,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10730,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10731,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10732,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10733,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10734,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10735,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10736,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10737,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10738,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10739,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10740,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10741,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10742,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10743,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10744,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10745,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10746,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10747,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10748,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10749,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10750,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10751,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10752,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10753,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10754,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10755,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10756,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10757,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10758,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10759,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10760,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10761,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10762,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10763,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10764,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10765,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10766,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10767,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10768,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10769,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10770,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10771,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10772,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10773,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10774,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10775,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10776,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10777,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10778,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10779,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10780,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10781,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10782,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10783,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10784,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10785,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10786,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10787,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10788,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10789,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10790,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10791,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10792,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10793,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10794,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10795,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10796,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10797,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10798,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10799,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10800,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10801,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10802,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10803,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10804,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10805,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10806,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10807,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10808,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10809,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10810,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10811,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10812,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10813,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10814,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10815,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10816,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10817,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10818,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10819,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10820,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10821,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10822,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10823,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10824,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10825,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10826,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10827,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10828,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10829,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10830,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10831,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10832,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10833,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10834,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10835,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10836,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10837,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10838,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10839,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10840,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10841,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10842,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10843,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10844,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10845,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10846,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10847,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10848,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10849,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10850,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10851,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10852,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10853,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10854,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10855,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10856,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10857,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10858,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10859,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10860,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10861,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10862,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10863,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10864,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "خواردنی سەرەکی", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10865,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10866,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10867,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10868,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10869,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10870,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10871,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10872,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10873,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10874,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10875,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10876,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10877,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10878,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10879,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10880,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10881,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10882,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10883,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10884,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "میوە و سەوزە", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10885,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10886,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10887,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10888,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10889,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10890,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10891,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10892,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10893,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10894,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10895,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10896,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10897,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10898,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10899,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10900,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10901,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10902,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10903,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10904,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10905,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10906,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10907,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10908,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10909,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10910,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10911,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10912,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10913,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10914,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10915,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10916,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10917,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10918,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10919,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10920,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10921,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10922,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10923,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10924,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10925,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10926,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10927,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10928,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10929,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10930,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10931,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10932,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10933,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10934,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10935,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10936,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10937,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10938,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10939,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10940,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10941,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10942,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10943,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10944,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10945,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10946,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10947,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10948,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10949,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10950,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10951,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10952,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10953,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10954,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10955,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10956,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10957,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10958,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10959,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10960,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10961,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10962,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10963,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10964,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10965,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10966,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10967,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10968,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10969,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10970,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10971,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10972,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10973,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10974,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10975,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10976,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10977,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10978,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10979,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10980,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10981,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10982,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10983,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10984,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10985,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10986,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10987,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10988,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10989,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10990,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10991,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10992,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10993,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10994,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10995,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10996,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10997,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10998,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10999,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 11000,
                columns: new[] { "Category", "SpeechPane" },
                values: new object[] { "کەلوپەل و بەهارات", 1 });
        }
    }
}
