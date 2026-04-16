using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Importance",
                table: "Words");

            migrationBuilder.AddColumn<int>(
                name: "SpeechPane",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 14);

            migrationBuilder.CreateTable(
                name: "WordMeans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Locate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordMeans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordMeans_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10000,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10001,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10002,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10003,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10004,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10005,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10006,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10007,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10008,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10009,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10010,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10011,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10012,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10013,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10014,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10015,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10016,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10017,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10018,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10019,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10020,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10021,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10022,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10023,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10024,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10025,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10026,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10027,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10028,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10029,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10030,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10031,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10032,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10033,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10034,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10035,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10036,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10037,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10038,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10039,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10040,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10041,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10042,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10043,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10044,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10045,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10046,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10047,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10048,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10049,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10050,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10051,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10052,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10053,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10054,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10055,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10056,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10057,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10058,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10059,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10060,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10061,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10062,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10063,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10064,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10065,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10066,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10067,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10068,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10069,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10070,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10071,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10072,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10073,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10074,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10075,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10076,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10077,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10078,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10079,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10080,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10081,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10082,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10083,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10084,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10085,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10086,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10087,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10088,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10089,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10090,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10091,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10092,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10093,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10094,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10095,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10096,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10097,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10098,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10099,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10100,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10101,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10102,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10103,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10104,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10105,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10106,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10107,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10108,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10109,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10110,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10111,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10112,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10113,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10114,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10115,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10116,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10117,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10118,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10119,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10120,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10121,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10122,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10123,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10124,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10125,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10126,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10127,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10128,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10129,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10130,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10131,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10132,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10133,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10134,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10135,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10136,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10137,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10138,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10139,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10140,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10141,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10142,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10143,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10144,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10145,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10146,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10147,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10148,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10149,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10150,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10151,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10152,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10153,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10154,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10155,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10156,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10157,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10158,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10159,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10160,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10161,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10162,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10163,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10164,
                columns: new[] { "CreatedAt", "Description", "Kurdish", "Meaning", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشككراو", "قەیسی وشككراو", "زانیاری ورد دەربارەی قەیسی وشككراو", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10165,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10166,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10167,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10168,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10169,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10170,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10171,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10172,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10173,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10174,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10175,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10176,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10177,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10178,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10179,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10180,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10181,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10182,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10183,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10184,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10185,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10186,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10187,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10188,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10189,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10190,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10191,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10192,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10193,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10194,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10195,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10196,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10197,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10198,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10199,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10200,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10201,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10202,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10203,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10204,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10205,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10206,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10207,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10208,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10209,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10210,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10211,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10212,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10213,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10214,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10215,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10216,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10217,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10218,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10219,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10220,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10221,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10222,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10223,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10224,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10225,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10226,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10227,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10228,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10229,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10230,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10231,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10232,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10233,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10234,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10235,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10236,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10237,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10238,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10239,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10240,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10241,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10242,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10243,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10244,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10245,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10246,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10247,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10248,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10249,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10250,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10251,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10252,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10253,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10254,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10255,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10256,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10257,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10258,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10259,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10260,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10261,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10262,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10263,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10264,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10265,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10266,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10267,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10268,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10269,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10270,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10271,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10272,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10273,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10274,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10275,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10276,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10277,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10278,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10279,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10280,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10281,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10282,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10283,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10284,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10285,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10286,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10287,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10288,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10289,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10290,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10291,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10292,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10293,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10294,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10295,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10296,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10297,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10298,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10299,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10300,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10301,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10302,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10303,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10304,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10305,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10306,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10307,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10308,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10309,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10310,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10311,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10312,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10313,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10314,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10315,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10316,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10317,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10318,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10319,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10320,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10321,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10322,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10323,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10324,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10325,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10326,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10327,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10328,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10329,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10330,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10331,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10332,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10333,
                columns: new[] { "CreatedAt", "Description", "Kurdish", "Meaning", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشككراو", "قەیسی وشككراو", "زانیاری ورد دەربارەی قەیسی وشككراو", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10334,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10335,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10336,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10337,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10338,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10339,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10340,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10341,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10342,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10343,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10344,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10345,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10346,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10347,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10348,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10349,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10350,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10351,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10352,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10353,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10354,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10355,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10356,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10357,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10358,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10359,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10360,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10361,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10362,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10363,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10364,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10365,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10366,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10367,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10368,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10369,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10370,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10371,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10372,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10373,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10374,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10375,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10376,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10377,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10378,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10379,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10380,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10381,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10382,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10383,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10384,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10385,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10386,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10387,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10388,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10389,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10390,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10391,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10392,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10393,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10394,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10395,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10396,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10397,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10398,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10399,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10400,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10401,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10402,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10403,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10404,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10405,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10406,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10407,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10408,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10409,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10410,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10411,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10412,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10413,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10414,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10415,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10416,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10417,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10418,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10419,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10420,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10421,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10422,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10423,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10424,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10425,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10426,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10427,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10428,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10429,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10430,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10431,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10432,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10433,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10434,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10435,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10436,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10437,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10438,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10439,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10440,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10441,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10442,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10443,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10444,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10445,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10446,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10447,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10448,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10449,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10450,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10451,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10452,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10453,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10454,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10455,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10456,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10457,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10458,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10459,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10460,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10461,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10462,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10463,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10464,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10465,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10466,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10467,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10468,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10469,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10470,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10471,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10472,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10473,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10474,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10475,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10476,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10477,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10478,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10479,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10480,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10481,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10482,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10483,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10484,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10485,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10486,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10487,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10488,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10489,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10490,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10491,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10492,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10493,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10494,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10495,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10496,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10497,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10498,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10499,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10500,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10501,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10502,
                columns: new[] { "CreatedAt", "Description", "Kurdish", "Meaning", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشككراو", "جۆری نایابی قەیسی وشككراو (10502)", "زانیاری ورد دەربارەی قەیسی وشككراو", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10503,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10504,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10505,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10506,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10507,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10508,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10509,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10510,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10511,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10512,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10513,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10514,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10515,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10516,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10517,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10518,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10519,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10520,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10521,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10522,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10523,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10524,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10525,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10526,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10527,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10528,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10529,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10530,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10531,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10532,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10533,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10534,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10535,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10536,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10537,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10538,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10539,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10540,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10541,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10542,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10543,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10544,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10545,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10546,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10547,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10548,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10549,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10550,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10551,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10552,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10553,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10554,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10555,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10556,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10557,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10558,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10559,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10560,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10561,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10562,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10563,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10564,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10565,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10566,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10567,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10568,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10569,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10570,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10571,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10572,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10573,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10574,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10575,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10576,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10577,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10578,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10579,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10580,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10581,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10582,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10583,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10584,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10585,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10586,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10587,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10588,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10589,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10590,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10591,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10592,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10593,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10594,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10595,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10596,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10597,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10598,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10599,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10600,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10601,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10602,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10603,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10604,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10605,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10606,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10607,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10608,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10609,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10610,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10611,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10612,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10613,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10614,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10615,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10616,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10617,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10618,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10619,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10620,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10621,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10622,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10623,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10624,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10625,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10626,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10627,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10628,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10629,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10630,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10631,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10632,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10633,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10634,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10635,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10636,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10637,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10638,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10639,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10640,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10641,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10642,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10643,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10644,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10645,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10646,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10647,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10648,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10649,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10650,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10651,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10652,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10653,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10654,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10655,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10656,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10657,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10658,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10659,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10660,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10661,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10662,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10663,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10664,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10665,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10666,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10667,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10668,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10669,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10670,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10671,
                columns: new[] { "CreatedAt", "Description", "Kurdish", "Meaning", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشككراو", "جۆری نایابی قەیسی وشككراو (10671)", "زانیاری ورد دەربارەی قەیسی وشككراو", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10672,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10673,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10674,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10675,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10676,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10677,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10678,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10679,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10680,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10681,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10682,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10683,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10684,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10685,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10686,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10687,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10688,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10689,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10690,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10691,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10692,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10693,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10694,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10695,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10696,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10697,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10698,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10699,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10700,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10701,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10702,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10703,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10704,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10705,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10706,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10707,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10708,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10709,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10710,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10711,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10712,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10713,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10714,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10715,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10716,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10717,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10718,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10719,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10720,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10721,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10722,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10723,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10724,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10725,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10726,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10727,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10728,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10729,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10730,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10731,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10732,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10733,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10734,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10735,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10736,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10737,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10738,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10739,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10740,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10741,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10742,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10743,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10744,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10745,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10746,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10747,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10748,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10749,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10750,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10751,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10752,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10753,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10754,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10755,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10756,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10757,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10758,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10759,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10760,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10761,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10762,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10763,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10764,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10765,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10766,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10767,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10768,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10769,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10770,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10771,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10772,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10773,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10774,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10775,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10776,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10777,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10778,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10779,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10780,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10781,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10782,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10783,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10784,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10785,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10786,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10787,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10788,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10789,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10790,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10791,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10792,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10793,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10794,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10795,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10796,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10797,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10798,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10799,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10800,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10801,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10802,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10803,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10804,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10805,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10806,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10807,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10808,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10809,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10810,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10811,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10812,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10813,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10814,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10815,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10816,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10817,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10818,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10819,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10820,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10821,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10822,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10823,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10824,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10825,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10826,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10827,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10828,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10829,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10830,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10831,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10832,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10833,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10834,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10835,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10836,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10837,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10838,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10839,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10840,
                columns: new[] { "CreatedAt", "Description", "Kurdish", "Meaning", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشككراو", "جۆری نایابی قەیسی وشككراو (10840)", "زانیاری ورد دەربارەی قەیسی وشككراو", 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10841,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10842,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10843,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10844,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10845,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10846,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10847,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10848,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10849,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10850,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10851,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10852,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10853,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10854,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10855,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10856,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10857,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10858,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10859,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10860,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10861,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10862,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10863,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10864,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10865,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10866,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10867,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10868,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10869,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10870,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10871,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10872,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10873,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10874,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10875,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10876,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10877,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10878,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10879,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10880,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10881,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10882,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10883,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10884,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10885,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10886,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10887,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10888,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10889,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10890,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10891,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10892,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10893,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10894,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10895,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10896,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10897,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10898,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10899,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10900,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10901,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10902,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10903,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10904,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10905,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10906,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10907,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10908,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10909,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10910,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10911,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10912,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10913,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10914,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10915,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10916,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10917,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10918,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10919,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10920,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10921,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10922,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10923,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10924,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10925,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10926,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10927,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10928,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10929,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10930,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10931,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10932,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10933,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10934,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10935,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10936,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10937,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10938,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10939,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10940,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10941,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10942,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10943,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10944,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10945,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10946,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10947,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10948,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10949,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10950,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10951,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10952,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10953,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10954,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10955,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10956,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10957,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10958,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10959,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10960,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10961,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10962,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10963,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10964,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10965,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10966,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10967,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10968,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10969,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10970,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10971,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10972,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10973,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10974,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10975,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10976,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10977,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10978,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10979,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10980,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10981,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10982,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10983,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10984,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10985,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10986,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10987,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10988,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10989,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10990,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10991,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10992,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10993,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10994,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10995,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10996,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10997,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10998,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10999,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 11000,
                columns: new[] { "CreatedAt", "SpeechPane" },
                values: new object[] { new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_WordMeans_WordId",
                table: "WordMeans",
                column: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordMeans");

            migrationBuilder.DropColumn(
                name: "SpeechPane",
                table: "Words");

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "Words",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Importance",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10000,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4283), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10001,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4286), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10002,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4287), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10003,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4288), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10004,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4290), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10005,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4291), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10006,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4293), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10007,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4294), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10008,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4295), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10009,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4296), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10010,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4297), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10011,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4299), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10012,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4300), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10013,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4300), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10014,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4301), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10015,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4302), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10016,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4304), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10017,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4305), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10018,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4306), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10019,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4307), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10020,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4309), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10021,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4310), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10022,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4341), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10023,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4343), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10024,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4344), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10025,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4345), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10026,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4346), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10027,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4348), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10028,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4348), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10029,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4349), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10030,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4350), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10031,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4351), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10032,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4352), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10033,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4354), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10034,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4355), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10035,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4356), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10036,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4357), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10037,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4358), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10038,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4359), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10039,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4360), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10040,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4361), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10041,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4362), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10042,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4363), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10043,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4364), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10044,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4365), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10045,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4366), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10046,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4391), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10047,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4392), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10048,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4393), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10049,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4394), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10050,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4395), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10051,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4396), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10052,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4397), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10053,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4398), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10054,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4399), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10055,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4400), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10056,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4401), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10057,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4402), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10058,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4403), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10059,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4404), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10060,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4405), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10061,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4406), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10062,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4406), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10063,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4408), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10064,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4409), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10065,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4411), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10066,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4412), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10067,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4413), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10068,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4438), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10069,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4440), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10070,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4441), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10071,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4442), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10072,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4443), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10073,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4444), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10074,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4445), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10075,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4447), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10076,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4448), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10077,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4449), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10078,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4450), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10079,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4450), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10080,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4451), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10081,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4452), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10082,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4453), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10083,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4454), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10084,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4455), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10085,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4456), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10086,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4457), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10087,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4458), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10088,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4459), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10089,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4460), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10090,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4461), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10091,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4462), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10092,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4463), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10093,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4464), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10094,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4488), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10095,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4489), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10096,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4491), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10097,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4492), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10098,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4493), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10099,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4494), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10100,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4495), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10101,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4496), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10102,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4497), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10103,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4498), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10104,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4498), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10105,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4499), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10106,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4500), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10107,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4501), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10108,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4502), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10109,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4503), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10110,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4504), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10111,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4505), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10112,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4506), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10113,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4507), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10114,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4508), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10115,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4509), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10116,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4510), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10117,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4510), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10118,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4512), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10119,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4512), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10120,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4536), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10121,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4537), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10122,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4539), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10123,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4540), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10124,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4541), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10125,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4542), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10126,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4543), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10127,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4544), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10128,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4545), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10129,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4547), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10130,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4548), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10131,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4549), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10132,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4550), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10133,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4551), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10134,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4552), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10135,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4553), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10136,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4554), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10137,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4555), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10138,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4556), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10139,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4579), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10140,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4580), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10141,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4581), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10142,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4582), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10143,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4583), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10144,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4584), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10145,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4585), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10146,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4586), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10147,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4587), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10148,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4588), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10149,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4589), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10150,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4590), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10151,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4591), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10152,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4592), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10153,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4593), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10154,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4594), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10155,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4595), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10156,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4596), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10157,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4597), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10158,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4598), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10159,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4599), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10160,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4600), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10161,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4601), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10162,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4601), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10163,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4617), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10164,
                columns: new[] { "ColorCode", "CreatedAt", "Description", "Importance", "Kurdish", "Meaning" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4619), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشککراو", 1, "قەیسی وشککراو", "زانیاری ورد دەربارەی قەیسی وشککراو" });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10165,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4620), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10166,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4621), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10167,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4623), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10168,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4624), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10169,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4625), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10170,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4626), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10171,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4627), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10172,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4628), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10173,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4629), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10174,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4630), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10175,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4631), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10176,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4632), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10177,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4632), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10178,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4633), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10179,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4634), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10180,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4635), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10181,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4636), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10182,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4637), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10183,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4638), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10184,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4639), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10185,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4640), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10186,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4641), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10187,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4642), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10188,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4672), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10189,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4673), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10190,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4674), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10191,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4676), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10192,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4677), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10193,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4678), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10194,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4679), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10195,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4680), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10196,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4681), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10197,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4682), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10198,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4683), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10199,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4684), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10200,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4684), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10201,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4685), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10202,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4686), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10203,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4687), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10204,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4688), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10205,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4689), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10206,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4690), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10207,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4691), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10208,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4692), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10209,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4693), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10210,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4694), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10211,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4695), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10212,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4696), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10213,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4697), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10214,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4728), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10215,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4729), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10216,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4730), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10217,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4731), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10218,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4732), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10219,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4733), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10220,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4734), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10221,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4735), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10222,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4736), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10223,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4737), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10224,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4738), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10225,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4739), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10226,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4740), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10227,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4741), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10228,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4742), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10229,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4743), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10230,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4744), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10231,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4745), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10232,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4746), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10233,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4747), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10234,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4747), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10235,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4748), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10236,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4749), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10237,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4750), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10238,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4751), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10239,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4752), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10240,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4776), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10241,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4777), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10242,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4778), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10243,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4779), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10244,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4780), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10245,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4781), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10246,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4782), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10247,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4783), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10248,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4784), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10249,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4785), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10250,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4786), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10251,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4787), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10252,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4788), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10253,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4789), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10254,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4790), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10255,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4791), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10256,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4792), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10257,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4816), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10258,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4817), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10259,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4819), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10260,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4820), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10261,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4821), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10262,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4822), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10263,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4823), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10264,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4825), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10265,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4825), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10266,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4826), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10267,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4827), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10268,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4828), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10269,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4829), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10270,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4830), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10271,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4831), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10272,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4832), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10273,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4833), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10274,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4834), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10275,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4835), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10276,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4836), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10277,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4836), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10278,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4862), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10279,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4863), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10280,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4864), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10281,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4865), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10282,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4866), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10283,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4867), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10284,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4868), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10285,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4868), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10286,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4869), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10287,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4870), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10288,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4871), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10289,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4872), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10290,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4873), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10291,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4874), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10292,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4875), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10293,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4876), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10294,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4877), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10295,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4877), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10296,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4878), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10297,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4879), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10298,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4880), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10299,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4881), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10300,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4882), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10301,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4883), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10302,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4884), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10303,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4908), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10304,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4909), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10305,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4911), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10306,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4912), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10307,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4913), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10308,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4914), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10309,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4915), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10310,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4916), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10311,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4917), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10312,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4918), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10313,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4918), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10314,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4919), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10315,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4920), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10316,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4921), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10317,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4922), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10318,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4923), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10319,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4924), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10320,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4925), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10321,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4926), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10322,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4927), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10323,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4928), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10324,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4929), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10325,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4930), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10326,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4931), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10327,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4932), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10328,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4956), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10329,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4957), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10330,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4958), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10331,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4959), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10332,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4960), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10333,
                columns: new[] { "ColorCode", "CreatedAt", "Description", "Importance", "Kurdish", "Meaning" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4961), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشککراو", 1, "قەیسی وشککراو", "زانیاری ورد دەربارەی قەیسی وشککراو" });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10334,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4962), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10335,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4963), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10336,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4964), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10337,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4965), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10338,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4966), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10339,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4968), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10340,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4968), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10341,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4969), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10342,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4970), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10343,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4971), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10344,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4972), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10345,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4973), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10346,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4974), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10347,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4975), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10348,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4976), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10349,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4977), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10350,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4978), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10351,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4979), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10352,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4979), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10353,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4980), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10354,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4995), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10355,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4996), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10356,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4997), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10357,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4998), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10358,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(4999), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10359,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5000), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10360,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5001), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10361,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5002), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10362,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5003), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10363,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5004), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10364,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5005), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10365,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5006), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10366,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5007), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10367,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5008), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10368,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5009), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10369,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5010), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10370,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5011), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10371,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5012), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10372,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5012), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10373,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5013), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10374,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5014), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10375,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5015), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10376,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5016), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10377,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5017), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10378,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5018), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10379,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5046), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10380,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5047), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10381,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5048), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10382,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5049), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10383,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5050), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10384,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5051), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10385,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5052), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10386,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5053), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10387,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5054), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10388,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5055), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10389,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5056), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10390,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5057), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10391,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5058), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10392,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5059), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10393,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5060), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10394,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5060), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10395,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5061), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10396,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5062), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10397,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5063), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10398,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5064), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10399,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5065), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10400,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5066), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10401,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5067), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10402,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5068), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10403,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5069), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10404,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5070), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10405,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5102), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10406,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5103), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10407,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5105), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10408,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5106), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10409,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5107), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10410,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5108), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10411,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5109), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10412,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5110), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10413,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5111), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10414,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5112), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10415,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5113), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10416,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5114), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10417,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5115), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10418,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5116), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10419,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5117), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10420,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5117), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10421,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5118), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10422,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5119), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10423,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5120), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10424,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5121), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10425,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5122), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10426,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5123), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10427,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5124), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10428,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5125), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10429,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5126), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10430,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5158), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10431,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5160), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10432,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5162), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10433,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5163), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10434,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5164), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10435,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5166), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10436,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5167), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10437,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5169), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10438,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5171), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10439,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5172), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10440,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5173), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10441,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5175), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10442,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5176), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10443,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5177), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10444,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5179), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10445,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5180), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10446,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5181), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10447,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5182), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10448,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5183), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10449,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5185), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10450,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5186), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10451,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5187), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10452,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5188), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10453,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5189), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10454,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5190), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10455,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5191), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10456,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5224), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10457,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5225), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10458,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5227), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10459,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5228), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10460,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5230), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10461,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5231), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10462,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5233), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10463,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5234), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10464,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5236), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10465,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5237), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10466,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5239), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10467,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5240), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10468,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5242), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10469,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5243), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10470,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5244), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10471,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5246), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10472,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5247), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10473,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5248), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10474,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5250), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10475,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5251), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10476,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5253), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10477,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5254), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10478,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5255), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10479,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5257), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10480,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5259), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10481,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5293), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10482,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5295), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10483,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5296), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10484,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5297), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10485,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5298), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10486,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5300), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10487,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5301), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10488,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5302), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10489,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5304), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10490,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5305), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10491,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5307), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10492,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5309), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10493,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5310), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10494,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5314), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10495,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5315), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10496,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5316), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10497,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5317), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10498,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5318), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10499,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5319), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10500,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5320), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10501,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5349), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10502,
                columns: new[] { "ColorCode", "CreatedAt", "Description", "Importance", "Kurdish", "Meaning" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5351), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشککراو", 1, "جۆری نایابی قەیسی وشککراو (10502)", "زانیاری ورد دەربارەی قەیسی وشککراو" });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10503,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5352), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10504,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5379), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10505,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5381), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10506,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5382), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10507,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5384), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10508,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5386), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10509,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5388), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10510,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5389), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10511,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5391), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10512,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5392), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10513,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5419), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10514,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5421), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10515,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5422), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10516,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5424), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10517,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5425), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10518,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5427), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10519,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5428), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10520,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5430), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10521,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5432), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10522,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5433), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10523,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5449), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10524,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5450), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10525,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5452), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10526,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5454), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10527,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5455), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10528,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5457), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10529,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5459), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10530,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5461), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10531,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5462), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10532,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5464), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10533,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5465), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10534,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5467), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10535,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5468), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10536,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5470), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10537,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5471), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10538,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5473), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10539,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5474), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10540,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5475), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10541,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5509), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10542,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5511), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10543,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5513), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10544,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5514), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10545,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5516), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10546,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5517), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10547,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5519), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10548,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5521), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10549,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5522), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10550,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5524), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10551,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5525), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10552,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5527), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10553,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5528), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10554,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5529), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10555,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5531), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10556,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5532), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10557,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5534), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10558,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5535), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10559,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5536), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10560,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5570), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10561,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5572), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10562,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5574), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10563,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5575), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10564,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5577), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10565,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5578), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10566,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5580), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10567,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5581), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10568,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5583), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10569,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5584), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10570,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5586), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10571,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5587), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10572,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5589), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10573,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5590), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10574,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5591), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10575,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5593), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10576,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5595), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10577,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5596), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10578,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5597), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10579,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5622), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10580,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5624), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10581,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5625), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10582,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5627), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10583,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5629), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10584,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5630), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10585,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5632), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10586,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5633), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10587,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5635), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10588,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5636), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10589,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5638), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10590,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5639), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10591,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5641), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10592,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5643), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10593,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5645), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10594,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5646), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10595,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5647), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10596,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5649), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10597,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5650), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10598,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5674), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10599,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5676), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10600,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5678), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10601,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5680), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10602,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5681), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10603,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5683), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10604,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5684), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10605,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5686), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10606,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5688), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10607,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5689), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10608,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5690), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10609,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5692), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10610,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5693), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10611,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5695), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10612,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5696), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10613,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5698), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10614,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5699), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10615,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5701), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10616,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5702), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10617,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5729), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10618,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5731), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10619,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5732), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10620,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5734), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10621,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5736), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10622,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5738), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10623,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5739), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10624,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5741), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10625,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5742), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10626,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5744), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10627,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5745), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10628,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5747), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10629,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5748), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10630,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5750), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10631,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5751), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10632,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5752), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10633,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5754), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10634,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5755), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10635,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5757), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10636,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5782), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10637,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5784), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10638,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5785), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10639,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5787), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10640,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5788), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10641,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5790), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10642,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5791), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10643,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5793), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10644,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5795), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10645,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5796), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10646,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5798), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10647,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5799), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10648,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5801), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10649,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5802), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10650,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5804), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10651,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5805), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10652,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5807), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10653,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5808), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10654,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5834), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10655,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5835), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10656,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5837), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10657,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5839), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10658,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5841), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10659,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5842), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10660,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5844), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10661,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5845), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10662,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5847), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10663,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5848), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10664,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5850), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10665,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5851), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10666,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5852), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10667,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5854), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10668,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5855), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10669,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5857), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10670,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5858), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10671,
                columns: new[] { "ColorCode", "CreatedAt", "Description", "Importance", "Kurdish", "Meaning" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5859), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشککراو", 1, "جۆری نایابی قەیسی وشککراو (10671)", "زانیاری ورد دەربارەی قەیسی وشککراو" });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10672,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5877), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10673,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5879), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10674,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5880), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10675,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5882), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10676,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5884), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10677,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5885), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10678,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5887), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10679,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5888), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10680,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5890), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10681,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5891), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10682,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5893), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10683,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5894), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10684,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5896), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10685,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5897), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10686,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5898), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10687,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5900), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10688,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5901), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10689,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5903), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10690,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5933), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10691,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5935), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10692,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5937), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10693,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5939), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10694,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5941), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10695,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5942), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10696,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5944), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10697,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5945), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10698,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5947), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10699,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5949), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10700,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5951), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10701,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5952), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10702,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5954), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10703,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5955), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10704,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5956), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10705,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5958), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10706,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5960), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10707,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5961), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10708,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5962), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10709,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5996), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10710,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5998), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10711,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(5999), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10712,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6001), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10713,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6003), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10714,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6004), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10715,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6006), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10716,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6007), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10717,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6009), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10718,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6010), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10719,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6012), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10720,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6013), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10721,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6015), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10722,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6016), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10723,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6018), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10724,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6019), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10725,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6020), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10726,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6022), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10727,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6023), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10728,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6047), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10729,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6049), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10730,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6051), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10731,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6052), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10732,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6054), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10733,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6055), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10734,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6057), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10735,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6058), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10736,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6060), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10737,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6061), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10738,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6063), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10739,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6064), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10740,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6066), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10741,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6067), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10742,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6069), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10743,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6071), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10744,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6072), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10745,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6074), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10746,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6075), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10747,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6099), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10748,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6101), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10749,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6103), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10750,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6104), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10751,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6106), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10752,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6108), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10753,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6109), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10754,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6111), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10755,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6112), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10756,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6114), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10757,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6115), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10758,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6116), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10759,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6118), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10760,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6119), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10761,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6121), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10762,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6122), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10763,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6124), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10764,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6125), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10765,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6127), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10766,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6152), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10767,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6154), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10768,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6155), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10769,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6157), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10770,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6159), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10771,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6160), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10772,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6162), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10773,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6163), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10774,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6165), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10775,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6166), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10776,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6167), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10777,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6169), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10778,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6171), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10779,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6172), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10780,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6173), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10781,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6175), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10782,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6177), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10783,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6178), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10784,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6200), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10785,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6203), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10786,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6204), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10787,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6206), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10788,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6208), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10789,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6209), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10790,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6211), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10791,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6212), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10792,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6214), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10793,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6215), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10794,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6217), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10795,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6219), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10796,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6220), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10797,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6221), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10798,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6223), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10799,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6224), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10800,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6226), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10801,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6227), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10802,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6229), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10803,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6254), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10804,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6255), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10805,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6257), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10806,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6259), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10807,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6260), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10808,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6262), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10809,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6263), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10810,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6265), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10811,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6267), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10812,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6268), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10813,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6269), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10814,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6271), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10815,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6272), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10816,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6274), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10817,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6275), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10818,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6277), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10819,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6278), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10820,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6280), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10821,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6294), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10822,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6296), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10823,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6298), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10824,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6300), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10825,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6301), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10826,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6303), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10827,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6304), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10828,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6306), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10829,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6307), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10830,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6309), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10831,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6310), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10832,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6311), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10833,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6313), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10834,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6314), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10835,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6316), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10836,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6317), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10837,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6319), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10838,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6320), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10839,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6348), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10840,
                columns: new[] { "ColorCode", "CreatedAt", "Description", "Importance", "Kurdish", "Meaning" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6350), "ئەم وشەیە پەیوەستە بە لقی قەیسی وشککراو", 1, "جۆری نایابی قەیسی وشککراو (10840)", "زانیاری ورد دەربارەی قەیسی وشککراو" });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10841,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6352), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10842,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6353), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10843,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6355), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10844,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6356), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10845,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6358), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10846,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6360), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10847,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6361), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10848,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6363), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10849,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6364), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10850,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6365), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10851,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6367), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10852,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6368), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10853,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6369), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10854,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6371), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10855,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6372), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10856,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6374), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10857,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6406), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10858,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6408), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10859,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6410), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10860,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6412), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10861,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6413), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10862,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6415), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10863,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6416), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10864,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6418), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10865,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6419), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10866,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6421), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10867,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6423), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10868,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6424), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10869,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6426), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10870,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6427), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10871,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6429), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10872,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6430), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10873,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6431), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10874,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6433), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10875,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6434), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10876,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6460), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10877,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6462), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10878,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6464), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10879,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6465), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10880,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6467), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10881,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6468), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10882,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6470), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10883,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6472), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10884,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6473), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10885,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6475), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10886,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6476), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10887,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6478), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10888,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6479), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10889,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6481), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10890,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6482), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10891,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6484), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10892,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6485), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10893,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6487), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10894,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6488), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10895,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6512), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10896,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6514), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10897,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6516), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10898,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6518), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10899,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6519), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10900,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6520), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10901,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6522), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10902,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6523), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10903,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6525), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10904,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6526), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10905,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6528), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10906,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6529), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10907,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6531), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10908,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6532), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10909,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6533), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10910,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6535), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10911,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6536), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10912,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6538), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10913,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6539), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10914,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6564), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10915,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6566), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10916,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6567), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10917,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6569), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10918,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6570), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10919,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6572), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10920,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6573), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10921,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6575), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10922,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6577), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10923,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6579), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10924,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6580), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10925,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6582), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10926,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6583), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10927,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6585), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10928,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6586), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10929,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6588), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10930,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6589), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10931,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6591), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10932,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6592), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10933,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6616), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10934,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6617), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10935,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6619), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10936,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6620), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10937,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6622), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10938,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6623), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10939,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6625), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10940,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6626), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10941,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6628), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10942,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6629), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10943,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6631), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10944,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6632), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10945,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6633), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10946,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6635), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10947,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6636), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10948,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6637), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10949,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6639), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10950,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6640), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10951,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6642), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10952,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6667), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10953,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6669), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10954,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6671), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10955,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6672), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10956,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6674), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10957,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6676), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10958,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6677), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10959,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6678), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10960,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6680), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10961,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6681), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10962,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6683), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10963,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6684), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10964,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6685), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10965,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6687), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10966,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6688), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10967,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6690), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10968,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6691), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10969,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6693), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10970,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6694), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10971,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6710), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10972,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6712), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10973,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6713), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10974,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6715), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10975,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6717), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10976,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6718), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10977,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6720), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10978,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6721), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10979,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6722), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10980,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6724), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10981,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6725), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10982,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6727), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10983,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6728), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10984,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6730), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10985,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6731), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10986,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6733), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10987,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6734), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10988,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6764), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10989,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6765), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10990,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6767), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10991,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6769), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10992,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6770), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10993,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6772), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10994,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6773), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10995,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6775), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10996,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6776), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10997,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6778), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10998,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6779), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10999,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6781), 1 });

            migrationBuilder.UpdateData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 11000,
                columns: new[] { "ColorCode", "CreatedAt", "Importance" },
                values: new object[] { null, new DateTime(2026, 4, 15, 15, 2, 54, 130, DateTimeKind.Utc).AddTicks(6782), 1 });
        }
    }
}
